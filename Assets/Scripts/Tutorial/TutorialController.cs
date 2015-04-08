﻿using UnityEngine;
using System.Collections;

public class TutorialController : MonoBehaviour {

	[SerializeField] private GameObject player;
	[SerializeField] private GameObject hud;
	[SerializeField] private GameObject camera;
	[SerializeField] private Vector3 spawn_point;
	[SerializeField] private Material floor_material;
	[SerializeField] private Material wall_material;
	[SerializeField] private float scale;

	private Stage stage;
	private Tile[,] loadedGrid;
	private ArrayList loadedRooms;

	void Awake () {
		var playerInstance = Instantiate(player, spawn_point, Quaternion.identity) as GameObject;
        var hudInstance = Instantiate(hud) as GameObject;
        var cameraInstance = Instantiate(camera, camera.transform.position, camera.transform.rotation) as GameObject;
        BuildGridAndRooms();
        StageBuilder.scale = scale;
		stage = new Stage(loadedGrid, loadedRooms);
		stage.Create();
		SetGridScales();
	}

	void BuildGridAndRooms () {
		loadedGrid = new Tile[17, 17];
		for(var i = 0; i <= 16; ++i) {
			for(var j = 0; j <= 16; ++j) {
				if(i == 12 && j == 4) {
					loadedGrid[i, j] = new Tile("Elevator", new Vector3(i, 0, j), floor_material, wall_material);
				} else if(i >= 11 && i <= 13 && j >= 3 && j <= 5) {
					loadedGrid[i, j] = new Tile("Exit", new Vector3(i, 0, j), floor_material, wall_material);
				} else if(i == 0 || (i == 8 && j != 12) || i == 16 || j == 0 || (j == 8 && i != 4 && i != 12) || j == 16) {
					loadedGrid[i, j] = new Tile("Rock", new Vector3(i, 0, j), floor_material, wall_material);
				} else {
					loadedGrid[i, j] = new Tile("Floor", new Vector3(i, 0, j), floor_material, wall_material);
				}
			}
		}
		loadedRooms = new ArrayList();
	}

	void SetGridScales () {}
}
