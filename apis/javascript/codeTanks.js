const $ = require("jquery");

exports.calculateFormula = function(formula, x) {
    if ((formula.minimum != null && x < formula.minimum) || (formula.maximum != null && x > formula.maximum)) {
        throw new Error("x is out of range");
    }
    switch (formula.formula) {
        case "polynomial":
            let value = 0;
            let xPow = 1;
            for (var i = formula.coefficients.length; i >= 0; --i) {
                value += formula.coefficients[i] * xPow;
                xPow *= x;
            }
            return value;
        case "exponential":
            let coefficient = formula.coefficient == null ? 1 : exports.calculateFormula(formula.coefficient, x);
            let base = formula.base == null ? 1 : exports.calculateFormula(formula.base, x);
            let exponent = formula.exponent == null ? 1 : exports.calculateFormula(formula.exponent, x);
            return coefficient * Math.pow(base, exponent);
        default:
            throw new Error("Invalid equation type");
    }
};

exports.IterativeTank = function(uri) {
    const _this = this;
    setTimeout(function() {
        let bldr = new exports.TankBuilder(uri);
        let tank = _this.build(bldr);
        let loop = function() {
            tank.hasStarted(function(data) {
                if (data) {
                    let pingDistance = NaN;
                    let location = null;
                    loop = function() {
                        let next = function() {
                            next = function() {
                                _this.run(tank, pingDistance, location, loop);
                            };
                            if (bldr.info.gps == 1) {
                                tank.readGps(function(data) {
                                    location = data;
                                    next();
                                });
                            } else {
                                next();
                            }
                        };
                        if (bldr.info.radar == 1) {
                            tank.readRadar(function(data) {
                                pingDistance = data;
                                next();
                            });
                        } else {
                            next();
                        }
                    };
                    loop();
                } else {
                    loop();
                }
            });
        };
        loop();
    });
};

exports.Tank = function(network, info) {
    movement = {
        "forward": {
            "direction": "forward",
            "rate": 0
        },
        "strafe": {
            "direction": "left",
            "rate": 0
        },
        "turn": {
            "direction": "clockwise",
            "rate": 0
        }
    };
    this.cannons = [];
    for (var i = 0; i < info.cannon; ++i) {
        this.cannons.push(new exports.TankCannon(network, i));
    }

    this.setForwardMovement = function(direction, rate, callback) {
        if (info.motor < 1) {
            throw new Error("Need at least one motor to move");
        }
        if (rate < 0 || rate > 1) {
            throw new Error("rate must be between 0 and 1");
        }
        movement.forward = {
            "direction": direction,
            "rate": rate
        };
        network.setMovement(movement, callback);
    };
    
    this.setTurnMovement = function(direction, rate, callback) {
        if (info.motor < 1) {
            throw new Error("Need at least two motors to turn");
        }
        if (rate < 0 || rate > 1) {
            throw new Error("rate must be between 0 and 1");
        }
        movement.turn = {
            "direction": direction,
            "rate": rate
        };
        network.setMovement(movement, callback);
    };
    
    this.setStrafeMovement = function(direction, rate, callback) {
        if (info.motor < 1) {
            throw new Error("Need at least four motors to strafe");
        }
        if (rate < 0 || rate > 1) {
            throw new Error("rate must be between 0 and 1");
        }
        movement.strafe = {
            "direction": direction,
            "rate": rate
        };
        network.setMovement(movement, callback);
    };

    this.readRadar = function(callback) {
        network.readRadar(function(data) {
            callback(data.distance);
        });
    };

    this.readGps = function(callback) {
        network.readGps(callback);
    };

    this.explode = function(callback) {
        network.explode(callback);
    };

    this.hasStarted = function(callback) {
        network.getInfo(function(data) {
            callback(data.state != "ready");
        });
    };
};

exports.TankBuilder = function(uri) {
    const network = new exports.TankNetwork(uri);
    let config;
    network.getConfig(function(data) {
        config = data;
    });
    let info = {
        "name": "",
        "motor": 0,
        "battery": 0,
        "cannon": 0,
        "radar": 0,
        "gps": 0,
        "explosives": 0
    };
    let built = false;

    function precall() {
        if (built) {
            throw new Error("The tank has already been built");
        }
    }

    function buy(newInfo) {
        let totalPrice = 0;
        totalPrice += exports.calculateFormula(config.costs.motor, newInfo.motor);
        totalPrice += exports.calculateFormula(config.costs.battery, newInfo.battery);
        totalPrice += exports.calculateFormula(config.costs.cannon, newInfo.cannon);
        totalPrice += exports.calculateFormula(config.costs.radar, newInfo.radar);
        totalPrice += exports.calculateFormula(config.costs.gps, newInfo.gps);
        totalPrice += exports.calculateFormula(config.costs.explosives, newInfo.explosives);
        if (totalPrice > config.funds) {
            throw new Error("The tank costs too much to build");
        }
        info = newInfo;
    }

    function cloneInfo() {
        return {
            "name": info.name,
            "motor": info.motor,
            "battery": info.battery,
            "cannon": info.cannon,
            "radar": info.radar,
            "gps": info.gps,
            "explosives": info.explosives
        };
    }

    this.name = function(name) {
        precall();
        info.name = name;
        return this;
    };

    this.buyMotors = function(num) {
        precall();
        let info = cloneInfo();
        info.motor += num;
        buy(info);
        return this;
    };

    this.buyBatteries = function(volts) {
        precall();
        let info = cloneInfo();
        info.battery += volts;
        buy(info);
        return this;
    };

    this.buyCannons = function(num) {
        precall();
        let info = cloneInfo();
        info.cannon += num;
        buy(info);
        return this;
    };

    this.buyRadar = function() {
        precall();
        let info = cloneInfo();
        info.radar = 1;
        buy(info);
        return this;
    };

    this.buyGPS = function() {
        precall();
        let info = cloneInfo();
        info.gps = 1;
        buy(info);
        return this;
    };

    this.buyExplosives = function() {
        precall();
        let info = cloneInfo();
        info.explosives = 1;
        buy(info);
        return this;
    };

    this.getNetwork = function() {
        return network;
    };

    this.build = function(callback) {
        precall();
        built = true;
        network.create(info, function() {
            callback(new exports.Tank(network, info));
        });
    };
};

exports.TankCannon = function(network, i) {
    this.charge = function(callback) {
        network.__internal_postAuthenticated(`/cannon/${i}/charge`, callback);
    };

    this.fire = function(callback) {
        network.__internal_postAuthenticated(`/cannon/${i}/fire`, callback);
    };
};

exports.TankNetwork = function(baseUrl) {
    let id;

    function post(endpoint, req, callback) {
        $.post(baseUrl + endpoint, JSON.stringify(req), function(data) {
            callback(JSON.parse(data));
        });
    }

    function postAuthenticated(endpoint, req, callback) {
        if (callback == null) {
            callback = req;
            req = {};
        }
        if (id == null) {
            throw new Error("The network has not authenticated yet");
        }
        req.id = id;
        post(endpoint, req, callback);
    }

    this.__internal_postAuthenticated = function(endpoint, callback) {
        postAuthenticated(endpoint, {}, callback);
    };

    function get(endpoint, callback) {
        $.get(baseUrl + endpoint, function(data) {
            callback(JSON.parse(data));
        });
    }

    this.getConfig = function(callback) {
        get("/config", callback);
    };

    this.getInfo = function(callback) {
        get("/info", callback);
    };

    this.getPlayers = function(authenticate, callback) {
        if (authenticate) {
            postAuthenticated("/players", callback);
        } else {
            get("/players", callback);
        }
    };

    this.getWalls = function(callback) {
        get("/field", callback);
    };

    this.start = function(callback) {
        get("/start", function(data) {
            id = data.id;
            callback();
        });
    };

    this.create = function(tank, callback) {
        post("/create", tank, function(data) {
            id = data.id;
            callback();
        });
    };

    this.getCannon = function(i) {
        return new exports.TankCannon(this, i);
    };

    this.setMovement = function(data, callback) {
        postAuthenticated("/move", data, callback);
    };

    this.readRadar = function(callback) {
        postAuthenticated("/radar", callback);
    };

    this.readGps = function(callback) {
        postAuthenticated("/gps", callback);
    };

    this.explode = function(callback) {
        postAuthenticated("/explode", callback);
    };
};
