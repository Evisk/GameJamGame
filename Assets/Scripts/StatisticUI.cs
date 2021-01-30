using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatisticUI : MonoBehaviour {
	private static readonly int MaxLevel = 5;

	public TMP_Text statName;
	public Button increaseButton;
	public Button decreaseButton;
	public RectTransform fillBarTransform;
	public StatisticName statNameEnum;

	private int currentLevel = 0;

	private void Awake() {
		this.increaseButton.onClick.AddListener((UnityEngine.Events.UnityAction)(() => {
			if(GameManager.Instance.IncreaseStatistic(statNameEnum))
            {
                
				this.currentLevel++;
				this.currentLevel = Mathf.Clamp(this.currentLevel, 0, MaxLevel);
				this.UpdateUI();
			}
			
		}));
		this.decreaseButton.onClick.AddListener((UnityEngine.Events.UnityAction)(() => {
            if (GameManager.Instance.DecreaseStatistic(statNameEnum))
            {
				this.currentLevel--;
				this.currentLevel = Mathf.Clamp(this.currentLevel, 0, MaxLevel);
				this.UpdateUI();
			}
			
		}));
		this.UpdateUI();
	}

	private void Start() {
		
	}
	
	private void Update() {
		
	}

	private void UpdateUI() {
		this.fillBarTransform.anchorMax = new Vector2((float)this.currentLevel / MaxLevel, 1f);
	}

	public void Initialize(StatisticName statName, int level, bool isMain) {
		this.statName.text = statName.ToString();
		this.statNameEnum = statName;
		this.currentLevel = level;
		if (isMain) {
			this.increaseButton.gameObject.SetActive(false);
			this.decreaseButton.gameObject.SetActive(false);
		} else {
			this.increaseButton.gameObject.SetActive(true);
			this.decreaseButton.gameObject.SetActive(true);
		}
		this.UpdateUI();
	}
}
