using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizerNavigator : MonoBehaviour {
	public int playerId;
	public PlayerChooser chooserMgr;
	public CustomizerNavigator nextCustomizer;
	public CustomizerNavigator prevCustomizer;

	public Color[] colors;
	public Sprite[] patterns;
	public bool startSelected;
	public bool isStripe;

	private SpriteRenderer spriteRenderer;
	private PlayerInfo playerRef;
	private int chosenOption = 0;
	private bool moved = false;
	private bool selected = false;

	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		playerRef = chooserMgr.GetPlayerInfo (playerId);

		if (colors.Length > 0) {
			UpdateColor ();
		} else {
			UpdateSprite ();
		}

		if (startSelected) {
			Highlight ();
		}
	}

	// Update is called once per frame
	void Update () {
		if (playerRef.isActive && selected) {
			if (!moved) {
				if (playerRef.input.MenuLeft) {
					ChooseNext (-1);
				} else if (playerRef.input.MenuRight) {
					ChooseNext (1);
				} else {
					moved = false;
				}
			}

			if (playerRef.input.MenuUp) {
				SelectNextCustomizer (-1);
			} else if (playerRef.input.MenuDown) {
				SelectNextCustomizer (1);
			}
		}
	}

	void Highlight () {
		selected = true;
		// Do something else
	}

	void UnHighlight () {
		selected = false;
		// Do something else
	}

	void ChooseNext (int direction) {
		moved = true;
		if (colors.Length > 0) {
			if (direction > 0) {
				++chosenOption;
				if (chosenOption >= colors.Length) {
					chosenOption = 0;
				}
			} else {
				--chosenOption;
				if (chosenOption < 0) {
					chosenOption = colors.Length - 1;
				}
			}

			UpdateColor ();
		} else {
			if (direction > 0) {
				++chosenOption;
				if (chosenOption >= patterns.Length) {
					chosenOption = 0;
				}
			} else {
				--chosenOption;
				if (chosenOption < 0) {
					chosenOption = patterns.Length - 1;
				}
			}

			UpdateSprite ();
		}
	}

	void UpdateSprite () {
		spriteRenderer.sprite = patterns [chosenOption];
		playerRef.settings.pattern = patterns [chosenOption];
	}

	void UpdateColor () {
		spriteRenderer.color = colors [chosenOption];
		if (isStripe) {
			playerRef.settings.stripe = colors [chosenOption];
		} else {
			playerRef.settings.tint = colors [chosenOption];
		}
	}

	public void SelectNextCustomizer (int direction) {
		if (direction > 0) {
			nextCustomizer.Highlight ();
		} else {
			prevCustomizer.Highlight ();
		}

		UnHighlight ();
	}
}
