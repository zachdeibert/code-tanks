package com.github.zachdeibert.codetanks.api;

import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.Reader;
import java.net.URI;

import org.apache.http.client.methods.CloseableHttpResponse;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.entity.StringEntity;
import org.apache.http.impl.client.CloseableHttpClient;
import org.apache.http.impl.client.HttpClients;

import com.github.zachdeibert.codetanks.api.model.ConfigData;
import com.github.zachdeibert.codetanks.api.model.GpsData;
import com.github.zachdeibert.codetanks.api.model.InfoData;
import com.github.zachdeibert.codetanks.api.model.LoginTokenContainer;
import com.github.zachdeibert.codetanks.api.model.MoveData;
import com.github.zachdeibert.codetanks.api.model.PlayerData;
import com.github.zachdeibert.codetanks.api.model.RadarData;
import com.github.zachdeibert.codetanks.api.model.SuccessfulResponse;
import com.github.zachdeibert.codetanks.api.model.TankInfo;
import com.github.zachdeibert.codetanks.api.model.WallData;
import com.github.zachdeibert.codetanks.api.model.InfoData.State;
import com.google.gson.Gson;

public final class TankNetwork implements AutoCloseable {
	private final URI baseUri;
	private final Gson gson;
	private final CloseableHttpClient client;
	private String id;

	private <TReq, TRes> TRes post(String endpoint, TReq req, Class<TRes> type) throws IOException {
		HttpPost post = new HttpPost(baseUri.resolve(endpoint));
		StringEntity entity = new StringEntity(gson.toJson(req));
		entity.setContentType("application/json");
		post.setEntity(entity);
		try (CloseableHttpResponse res = client.execute(post);
				InputStream stream = res.getEntity().getContent();
				Reader reader = new InputStreamReader(stream)) {
			return gson.fromJson(reader, type);
		}
	}

	private <TReq extends LoginTokenContainer, TRes> TRes postAuthenticated(String endpoint, TReq req, Class<TRes> type)
			throws IOException {
		if (id == null) {
			throw new IllegalStateException("The network has not authenticated yet");
		}
		req.id = id;
		return post(endpoint, req, type);
	}

	<TRes> TRes postAuthenticated(String endpoint, Class<TRes> type) throws IOException {
		return postAuthenticated(endpoint, new LoginTokenContainer(), type);
	}

	private <T> T get(String endpoint, Class<T> type) throws IOException {
		HttpGet get = new HttpGet(baseUri.resolve(endpoint));
		try (CloseableHttpResponse res = client.execute(get);
				InputStream stream = res.getEntity().getContent();
				Reader reader = new InputStreamReader(stream)) {
			return gson.fromJson(reader, type);
		}
	}

	public ConfigData getConfig() throws IOException {
		return get("/config", ConfigData.class);
	}

	public InfoData getInfo() throws IOException {
		return get("/info", InfoData.class);
	}

	public PlayerData getPlayers(boolean authenticate) throws IOException {
		if (authenticate) {
			return postAuthenticated("/players", PlayerData.class);
		} else {
			return get("/players", PlayerData.class);
		}
	}

	public WallData getWalls() throws IOException {
		return get("/field", WallData.class);
	}

	public void start() throws IOException {
		id = get("/start", LoginTokenContainer.class).id;
	}

	public void create(TankInfo tank) throws IOException {
		id = post("/create", tank, LoginTokenContainer.class).id;
	}

	public TankCannon getCannon(int i) {
		return new TankCannon(this, i);
	}

	public void setMovement(MoveData data) throws IOException {
		postAuthenticated("/move", data, SuccessfulResponse.class);
	}

	public RadarData readRadar() throws IOException {
		return postAuthenticated("/radar", RadarData.class);
	}

	public GpsData readGps() throws IOException {
		return postAuthenticated("/gps", GpsData.class);
	}

	public void explode() throws IOException {
		postAuthenticated("/explode", SuccessfulResponse.class);
	}

	@Override
	public void close() throws IOException {
		client.close();
	}

	public TankNetwork(final URI baseUrl) throws IOException {
		this.baseUri = baseUrl;
		gson = new Gson();
		client = HttpClients.createDefault();
		InfoData info = getInfo();
		if (info.version.length != 2 || info.version[0] != 1) {
			throw new IOException("Invalid protocol version");
		}
		if (info.state != State.ready) {
			throw new IOException("The server is not ready");
		}
	}
}
