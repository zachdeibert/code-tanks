using System;
using System.Threading;
using Com.GitHub.ZachDeibert.CodeTanks.Model;
using Com.GitHub.ZachDeibert.CodeTanks.Server;
using Com.GitHub.ZachDeibert.CodeTanks.Simulator;

namespace Com.GitHub.ZachDeibert.CodeTanks {
    class Program {
        static void Main(string[] args) {
            int port;
            if (args.Length == 0 || !int.TryParse(args[0], out port)) {
                port = 4242;
            }
            GameModel model = new GameModel {
                Configuration = new ConfigData {
                    TotalFunds = 1000,
                    Costs = new ConfigData.CostData {
                        MotorCost = new Formula {
                            Minimum = 0,
                            EquationType = Formula.Type.Polynomial,
                            Coefficients = new double[] {
                                75,
                                0
                            }
                        },
                        BatteryCost = new Formula {
                            Minimum = 10,
                            EquationType = Formula.Type.Exponential,
                            Base = new Formula {
                                EquationType = Formula.Type.Polynomial,
                                Coefficients = new double[] {
                                    2
                                }
                            },
                            Exponent = new Formula {
                                EquationType = Formula.Type.Polynomial,
                                Coefficients = new double[] {
                                    0.6,
                                    0
                                }
                            }
                        },
                        CannonCost = new Formula {
                            Minimum = 0,
                            EquationType = Formula.Type.Polynomial,
                            Coefficients = new double[] {
                                100,
                                0
                            }
                        },
                        RadarCost = new Formula {
                            Minimum = 0,
                            Maximum = 1,
                            EquationType = Formula.Type.Polynomial,
                            Coefficients = new double[] {
                                250,
                                0
                            }
                        },
                        GPSCost = new Formula {
                            Minimum = 0,
                            Maximum = 1,
                            EquationType = Formula.Type.Polynomial,
                            Coefficients = new double[] {
                                125,
                                0
                            }
                        },
                        ExplosivesCost = new Formula {
                            Minimum = 0,
                            Maximum = 1,
                            EquationType = Formula.Type.Polynomial,
                            Coefficients = new double[] {
                                32,
                                0
                            }
                        }
                    },
                    Speeds = new ConfigData.SpeedData {
                        BulletSpeed = new Formula {
                            EquationType = Formula.Type.Exponential,
                            Base = new Formula {
                                EquationType = Formula.Type.Polynomial,
                                Coefficients = new double[] {
                                    2
                                }
                            },
                            Exponent = new Formula {
                                EquationType = Formula.Type.Polynomial,
                                Coefficients = new double[] {
                                    0.5,
                                    0
                                }
                            }
                        },
                        TankDriveSpeed = new Formula {
                            EquationType = Formula.Type.Exponential,
                            Coefficient = new Formula {
                                EquationType = Formula.Type.Polynomial,
                                Coefficients = new double[] {
                                    0.1
                                }
                            },
                            Base = new Formula {
                                EquationType = Formula.Type.Polynomial,
                                Coefficients = new double[] {
                                    1.1
                                }
                            },
                            Exponent = new Formula {
                                EquationType = Formula.Type.Polynomial,
                                Coefficients = new double[] {
                                    0.5,
                                    0
                                }
                            }
                        },
                        TankTurnSpeed = new Formula {
                            EquationType = Formula.Type.Exponential,
                            Coefficient = new Formula {
                                EquationType = Formula.Type.Polynomial,
                                Coefficients = new double[] {
                                    0.1
                                }
                            },
                            Base = new Formula {
                                EquationType = Formula.Type.Polynomial,
                                Coefficients = new double[] {
                                    1.1
                                }
                            },
                            Exponent = new Formula {
                                EquationType = Formula.Type.Polynomial,
                                Coefficients = new double[] {
                                    0.5,
                                    0
                                }
                            }
                        }
                    },
                    Explosions = new ConfigData.ExplosionData {
                        Size = new Formula {
                            EquationType = Formula.Type.Polynomial,
                            Coefficients = new double[] {
                                0.02,
                                0.02
                            }
                        },
                        Duration = 1000
                    },
                    TankSize = 0.04
                },
                Walls = new WallData {
                    Walls = {
                        new WallInfo {
                            X0 = 0,
                            X1 = 1,
                            Y0 = 0,
                            Y1 = 0
                        },
                        new WallInfo {
                            X0 = 0,
                            X1 = 1,
                            Y0 = 1,
                            Y1 = 1
                        },
                        new WallInfo {
                            X0 = 0,
                            X1 = 0,
                            Y0 = 0,
                            Y1 = 1
                        },
                        new WallInfo {
                            X0 = 1,
                            X1 = 1,
                            Y0 = 0,
                            Y1 = 1
                        }
                    }
                }
            };
            using (GameSimulator simulator = new GameSimulator(model)) {
                using (WebServer server = new WebServer(model, port)) {
                    Console.WriteLine("Listening on http://localhost:{0}/", port);
                    if (Console.IsInputRedirected || Console.IsOutputRedirected || Console.IsErrorRedirected) {
                        Thread.Sleep(int.MaxValue);
                    } else {
                        Console.WriteLine("Press any key to stop the server.");
                        Console.ReadKey();
                    }
                }
            }
        }
    }
}
