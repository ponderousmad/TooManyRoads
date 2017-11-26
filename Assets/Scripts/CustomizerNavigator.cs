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
	public GameObject highlight;
	public bool startSelected;
	public bool isStripe;
	public SpriteRenderer customSprite;

	private SpriteRenderer spriteRenderer;
	private PlayerInfo playerRef;
	public int chosenOption = 0;
	private bool moved = false;
	private bool movedUp = false;
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
		} else {
			UnHighlight ();
		}
	}

	// Update is called once per frame
	void Update () {
		if (playerRef.isActive && selected) {
			if (playerRef.input.MenuLeft) {
				if (!moved) {
					ChooseNext (-1);
				}
			} else if (playerRef.input.MenuRight) {
				if (!moved) {
					ChooseNext (1);
				}
			} else {
				moved = false;
			}

			if (playerRef.input.MenuUp) {
				if (!movedUp) {
					SelectNextCustomizer (-1);
				}
			} else if (playerRef.input.MenuDown) {
				if (!movedUp) {
					SelectNextCustomizer (1);
				}
			} else {
				movedUp = false;
			}
		}
	}

	void Highlight () {
		selected = true;
		movedUp = true;
		moved = true;
		highlight.SetActive (true);
	}

	void UnHighlight () {
		selected = false;
		highlight.SetActive (false);
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
			customSprite.material.SetColor ("_Accent", spriteRenderer.color);
		} else {
			playerRef.settings.tint = colors [chosenOption];
			customSprite.material.SetColor ("_Color", spriteRenderer.color);
		}
	}

	void SelectNextCustomizer (int direction) {
		movedUp = true;
		if (direction > 0) {
			nextCustomizer.Highlight ();
		} else {
			prevCustomizer.Highlight ();
		}

		UnHighlight ();
	}
}
