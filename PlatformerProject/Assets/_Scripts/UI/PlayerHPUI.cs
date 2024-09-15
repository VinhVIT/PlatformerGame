using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHPUI : MonoBehaviour
{
    [SerializeField] private GameObject portrait;
    private PlayerStats playerStats;
    private Combat combat;

    [Header("Health Settings")]
    [SerializeField, Range(1, 8)] private int maxHealth;
    [SerializeField] private GameObject HPContainer;
    [SerializeField] private RectTransform healthBarBackground;
    [SerializeField] private GameObject healthBarBreak;
    [SerializeField] private List<GameObject> hpList = new List<GameObject>();
    [SerializeField] private List<GameObject> hpBreakList = new List<GameObject>();

    [Header("Animation Ref")]
    [SerializeField] private GameObject HPParticle;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private Image healthLossBar;
    [SerializeField] private GameObject animateHPPrefab;
    [SerializeField] private float healthLossAnimationDuration = 0.5f;
    private Animator healthBarAnimator;
    private Coroutine healthLossCoroutine;
    private const float HP_OFFSET = 66;// this value is the width of the HP prefab
    private float hpLossValue;
    private int currentHealth;
    private List<GameObject> currentHPList = new List<GameObject>();

    [Header("Energy Settings")]
    [SerializeField] private Slider energySlider;

    [Header("Text Displays")]
    [SerializeField] private TextMeshProUGUI maxHPText;
    [SerializeField] private TextMeshProUGUI currentHPText;
    [SerializeField] private TextMeshProUGUI maxEnergyText;
    [SerializeField] private TextMeshProUGUI currentEnergyText;

    private void Awake()
    {
        healthBarAnimator = healthBar.GetComponent<Animator>();
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerStats>();
        combat = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Combat>();
    }

    private void Start()
    {
        SetupInitialState();
        combat.OnGotDamaged += DecreaseHealth;
        playerStats.Health.OnValueIncreased += IncreaseHealth;
        playerStats.OnEnergyChanged += UpdateEnergySlider;

        InitializeEnergySlider();
    }
    private void OnDestroy()
    {
        combat.OnGotDamaged -= DecreaseHealth;
        playerStats.OnEnergyChanged -= UpdateEnergySlider;
        playerStats.Health.OnValueIncreased -= IncreaseHealth;
    }
    #region HP
    private void SetupInitialState()
    {
        maxHealth = playerStats.Health.CurrentValue;
        currentHealth = maxHealth;
        hpLossValue = 1f / maxHealth + 0.05f;
        InitHPBar();
        currentHPList = GetCurrentHPList();
        UpdateHPTexts();
        UpdateEnergyTexts();
    }

    private void InitHPBar()
    {
        List<GameObject> currenthpList = GetHPList();

        if (maxHealth > currenthpList.Count)
        {
            for (int i = 1; i <= maxHealth - currenthpList.Count; i++)
            {
                hpList[i].SetActive(true);
                IncreaseRightOffset(HP_OFFSET);
            }
        }
    }

    private void InitBreakHP()
    {
        if (maxHealth > 3)
        {
            for (int i = 0; i < maxHealth - 3; i++)
            {
                hpBreakList[i].SetActive(true);
            }
        }
    }

    private List<GameObject> GetHPList()
    {
        List<GameObject> hpList = new List<GameObject>();
        foreach (Transform child in HPContainer.transform)
        {
            if (child.gameObject.activeSelf)
            {
                hpList.Add(child.gameObject);
            }
        }
        return hpList;
    }

    private List<GameObject> GetCurrentHPList()
    {
        List<GameObject> currentHPList = new List<GameObject>();
        for (int i = 0; i < hpList.Count; i++)
        {
            if (hpList[i].activeSelf)
            {
                currentHPList.Add(hpList[i]);
            }
        }
        return currentHPList;
    }

    private void IncreaseRightOffset(float amount)
    {
        Vector2 offsetMin = healthBarBackground.offsetMin;
        Vector2 offsetMax = healthBarBackground.offsetMax;
        offsetMax.x += amount;
        healthBarBackground.offsetMax = offsetMax;
    }

    public void DecreaseHealth(int amount)
    {
        TriggerHurtAnimations();
        UpdateHealthLossBar();
        UpdateCurrentHealth(amount);
        UpdateHealthUI();
        UpdateHPTexts();
    }

    private void UpdateHealthLossBar()
    {
        if (healthLossCoroutine != null)
        {
            StopCoroutine(healthLossCoroutine);
        }
        healthLossCoroutine = StartCoroutine(DecreaseHealthLossBar(hpLossValue));
    }

    private void UpdateCurrentHealth(int amount)
    {
        currentHealth = Mathf.Max(0, currentHealth - amount);
    }

    private void UpdateHealthUI()
    {
        if (currentHealth == 1)//critical health
        {
            animateHPPrefab.SetActive(false);
            healthBarAnimator.SetBool("injured", true);
        }
        else if (currentHealth <= 0)//death
        {
            PlayerDeathHandler();
            healthBarAnimator.SetBool("injured", false);
        }
        else//losing health
        {
            currentHPList[currentHealth - 1].SetActive(false);
            UpdateHPParticlePosition();
            HPParticle.GetComponent<Animator>().SetTrigger("hurt");
            healthBarAnimator.SetBool("injured", false);
        }
    }
    private void UpdateHPParticlePosition()
    {
        Vector3 newPosition = currentHPList[currentHealth - 1].transform.position;
        newPosition.y = HPParticle.transform.position.y;
        newPosition.z = HPParticle.transform.position.z;
        HPParticle.transform.position = newPosition;
    }

    public void IncreaseHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
            animateHPPrefab.GetComponent<Animator>().SetBool("hurt", false);
            currentHPList.Last().SetActive(true);
        }
        else
        {
            currentHPList[currentHealth - 2].SetActive(true);
        }
        UpdateHPTexts();
    }

    private void PlayerDeathHandler()
    {
        hpList[currentHealth].SetActive(false);
        portrait.GetComponent<Animator>().SetBool("break", true);

        healthBarBreak.SetActive(true);
        InitBreakHP();
    }

    private IEnumerator DecreaseHealthLossBar(float amount)
    {
        float startFillAmount = healthLossBar.fillAmount;
        float endFillAmount = startFillAmount - amount;
        float elapsedTime = 0f;

        while (elapsedTime < healthLossAnimationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / healthLossAnimationDuration;
            healthLossBar.fillAmount = Mathf.Lerp(startFillAmount, endFillAmount, t);
            yield return null;
        }

        healthLossBar.fillAmount = endFillAmount;
    }
    private void UpdateHPTexts()
    {
        maxHPText.text = (maxHealth * 10).ToString();
        currentHPText.text = $"{currentHealth * 10} /";
    }
    private void TriggerHurtAnimations()
    {
        animateHPPrefab.GetComponent<Animator>().SetBool("hurt", true);
        healthBarAnimator.SetTrigger("hurt");
    }
    #endregion
    #region Energy
    private void InitializeEnergySlider()
    {
        energySlider.minValue = playerStats.Energy.CurrentValue;
        energySlider.maxValue = playerStats.Energy.MaxValue;
        UpdateEnergySlider(playerStats.Energy.CurrentValue);
    }

    private void UpdateEnergySlider(int newEnergy)
    {
        energySlider.value = newEnergy;
        currentEnergyText.text = $"{newEnergy} /";
    }

    private void UpdateEnergyTexts()
    {
        maxEnergyText.text = playerStats.Energy.MaxValue.ToString();
        currentEnergyText.text = $"{playerStats.Energy.CurrentValue} /";
    }
    #endregion


}