# Anonymous Tank Simulator

This is an application that allows people to write anonymous tank code, then the
simulator will battle them and display the results.
Tanks communicate with a REST API, so they can be written in any language that
supports making HTTP requests.

## Usage

First, a server must be started.
Inside this repo, there is a C# server implementation that can be compiled and
run on any platform that supports .NET.
Then, each player can start their tank program and connect it to the server, and
they can go to [the web viewer](https://zachdeibert.github.io/code-tanks) to see
how the game is going.

## Protocol

### Unauthenticated Requests

None of these requests need to be authenticated, so their only requirement is
that they are `GET` requests.

#### GET /config

```http
GET /config HTTP/1.1

HTTP/1.1 200 OK
Content-Type: application/json
Content-Length: 3232

{
    "funds": 1000,
    "costs": {
        "motor": {
            "min": 0,
            "max": null,
            "formula": "polynomial",
            "coefficients": [
                75,
                0
            ]
        },
        "battery": {
            "min": 10,
            "max": null,
            "formula": "exponential",
            "base": {
                "formula": "polynomial",
                "coefficients": [
                    2
                ]
            },
            "exponent": {
                "formula": "polynomial",
                "coefficients": [
                    0.6,
                    0
                ]
            }
        },
        "cannon": {
            "min": 0,
            "max": null,
            "formula": "polynomial",
            "coefficients": [
                100,
                0
            ]
        },
        "radar": {
            "min": 0,
            "max": 1,
            "formula": "polynomial",
            "coefficients": [
                250,
                0
            ]
        },
        "gps": {
            "min": 0,
            "max": 1,
            "formula": "polynomial",
            "coefficients": [
                125,
                0
            ]
        },
        "explosives": {
            "min": 0,
            "max": 1,
            "formula": "polynomial",
            "coefficients": [
                32,
                0
            ]
        }
    },
    "speeds": {
        "bullet": {
            "formula": "exponential",
            "base": {
                "formula": "polynomial",
                "coefficients": [
                    2
                ]
            },
            "exponent": {
                "formula": "polynomial",
                "coefficients": [
                    0.5,
                    0
                ]
            }
        },
        "tankDrive": {
            "formula": "exponential",
            "coefficient": {
                "formula": "polynomial",
                "coefficients": [
                    0.1
                ]
            },
            "base": {
                "formula": "polynomial",
                "coefficients": [
                    1.1
                ]
            },
            "exponent": {
                "formula": "polynomial",
                "coefficients": [
                    0.5,
                    0
                ]
            }
        },
        "tankTurn": {
            "formula": "exponential",
            "coefficient": {
                "formula": "polynomial",
                "coefficients": [
                    0.1
                ]
            },
            "base": {
                "formula": "polynomial",
                "coefficients": [
                    1.1
                ]
            },
            "exponent": {
                "formula": "polynomial",
                "coefficients": [
                    0.5,
                    0
                ]
            }
        }
    },
    "explosions": {
        "size": {
            "formula": "polynomial",
            "coefficients": [
                0.02,
                0.02
            ]
        },
        "duration": 1000
    },
    "tankSize": 0.04
}
```

#### GET /info

```http
GET /info HTTP/1.1

HTTP/1.1 200 OK
Content-Type: application/json
Content-Length: 70

{
    "version": [
        1,
        0
    ],
    "state": "ready"
}
```

#### GET /players

```http
GET /players HTTP/1.1

HTTP/1.1 200 OK
Content-Type: application/json
Content-Length: 204

{
    "players": [
        {
            "name": "Zach Deibert",
            "alive": true,
            "score": 42
        }
    ]
}
```

#### GET /field

```http
GET /field HTTP/1.1

HTTP/1.1 200 OK
Content-Type: application/json
Content-Length:

{
    "walls": [
        {
            "x0": 0,
            "x1": 1,
            "y0": 0,
            "y1": 0
        },
        {
            "x0": 0,
            "x1": 1,
            "y0": 1,
            "y1": 1
        },
        {
            "x0": 0,
            "x1": 0,
            "y0": 0,
            "y1": 1
        },
        {
            "x0": 1,
            "x1": 1,
            "y0": 0,
            "y1": 1
        }
    ]
}
```

#### GET /start

```http
GET /start HTTP/1.1

HTTP/1.1 200 OK
Content-Type: application/json
Content-Length: 55

{
    "id": "{1E413AE6-09AD-4E56-947D-3A2F033D8221}"
}
```

### Authenticated Requests

All of these requests must be authenticated and sent via a `POST` request with
a JSON body.

#### POST /players

```http
POST /players HTTP/1.1
Content-Type: application/json
Content-Length: 55

{
    "id": "{1E413AE6-09AD-4E56-947D-3A2F033D8221}"
}

HTTP/1.1 200 OK
Content-Type: application/json
Content-Length: 204

{
    "players": [
        {
            "name": "Zach Deibert",
            "alive": true,
            "score": 42,
            "x": 0.2,
            "y": 0.7,
            "rot": 1.412
        }
    ]
}
```

#### POST /create

```http
POST /create HTTP/1.1
Content-Type: application/json
Content-Length: 134

{
    "name": "Zach Deibert",
    "motor": 4,
    "battery": 10,
    "cannon": 1,
    "radar": 1,
    "gps": 0,
    "explosives": 0
}

HTTP/1.1 200 OK
Content-Type: application/json
Content-Length: 55

{
    "id": "{64623648-DFE0-4EAF-8F93-6B9A039AC9D7}"
}
```

#### POST /cannon/%d/charge

```http
POST /cannon/0/charge HTTP/1.1
Content-Type: application/json
Content-Length: 55

{
    "id": "{64623648-DFE0-4EAF-8F93-6B9A039AC9D7}"
}

HTTP/1.1 200 OK
Content-Type: application/json
Content-Length: 24

{
    "success": true
}
```

#### POST /cannon/%d/fire

```http
POST /cannon/0/fire HTTP/1.1
Content-Type: application/json
Content-Length: 55

{
    "id": "{64623648-DFE0-4EAF-8F93-6B9A039AC9D7}"
}

HTTP/1.1 200 OK
Content-Type: application/json
Content-Length: 24

{
    "success": true
}
```

#### POST /move

```http
POST /move HTTP/1.1
Content-Type: application/json
Content-Length: 286

{
    "id": "{64623648-DFE0-4EAF-8F93-6B9A039AC9D7}",
    "forward": {
        "direction": "forward",
        "rate": 0.7
    },
    "strafe": {
        "direction": "right",
        "rate": 0.1
    },
    "turn": {
        "direction": "counterclockwise",
        "rate": 0.6
    }
}

HTTP/1.1 200 OK
Content-Type: application/json
Content-Length: 24

{
    "success": true
}
```

#### POST /radar

```http
POST /radar HTTP/1.1
Content-Type: application/json
Content-Length: 55

{
    "id": "{64623648-DFE0-4EAF-8F93-6B9A039AC9D7}"
}

HTTP/1.1 200 OK
Content-Type: application/json
Content-Length: 26

{
    "distance": 0.542
}
```

#### POST /gps

```http
POST /gps HTTP/1.1
Content-Type: application/json
Content-Length: 55

{
    "id": "{64623648-DFE0-4EAF-8F93-6B9A039AC9D7}"
}

HTTP/1.1 200 OK
Content-Type: application/json
Content-Length: 49

{
    "x": 0.2,
    "y": 0.7,
    "rot": 1.412
}
```

#### POST /explode

```http
POST /gps HTTP/1.1
Content-Type: application/json
Content-Length: 55

{
    "id": "{64623648-DFE0-4EAF-8F93-6B9A039AC9D7}"
}

HTTP/1.1 200 OK
Content-Type: application/json
Content-Length: 24

{
    "success": true
}
```

### Errors

Any request can return an error instead of the expected content.

```http
HTTP/1.1 200 OK
Content-Type: application/json
Content-Length:

{
    "error": true,
    "type": "DeadTankException",
    "message": "Your tank is dead, so it cannot move."
}
```
