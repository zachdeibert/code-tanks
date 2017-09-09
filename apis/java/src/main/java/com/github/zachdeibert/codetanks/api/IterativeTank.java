package com.github.zachdeibert.codetanks.api;

import java.io.IOException;
import java.net.URI;

import com.github.zachdeibert.codetanks.api.model.GpsData;

public abstract class IterativeTank {
	final Thread thread;
	final URI uri;

	public abstract Tank build(TankBuilder builder) throws IOException;

	public abstract void run(Tank tank, double pingDistance, GpsData location) throws IOException;

	private void loop() {
		try {
			TankBuilder bldr = new TankBuilder(uri);
			Tank tank = build(bldr);
			while (!tank.hasStarted()) {
				Thread.sleep(100);
			}
			double pingDistance = 1 / 0;
			GpsData location = null;
			while (true) {
				if (bldr.info.radar == 1) {
					pingDistance = tank.readRadar();
				}
				if (bldr.info.gps == 1) {
					location = tank.readGps();
				}
				run(tank, pingDistance, location);
			}
		} catch (Exception ex) {
			ex.printStackTrace();
		}
	}
	
	public void join() throws InterruptedException {
		thread.join();
	}

	protected IterativeTank(URI uri) {
		this.uri = uri;
		thread = new Thread(() -> loop());
		thread.start();
	}
}
