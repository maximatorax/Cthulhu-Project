using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsSystem : MonoBehaviour, IStatsSystem
{

    public int level = 1;

    public int Strength = 1;

    public int Agility = 1;

    public int Constitution = 1;

    public int Wisdom = 1;

    public int Intelligence = 1;

    public int StatPoints;

    public bool leveling;

    public int exp = 0;

    public int expToLevel = 100;

    public TMP_Text StrengthText;
    public TMP_Text AgilityText;
    public TMP_Text ConstitutionText;
    public TMP_Text WisdomText;
    public TMP_Text IntelligenceText;
    public TMP_Text StatPointsText;
    public TMP_Text LevelText;

    public GameObject LevelUpPanel;
    public GameObject LevelUpButton;

    public Scrollbar ExpBar;
    private TMP_Text ExpText;

    private PlayerHealthSystem playerHealthSystem;
    private PlayerAttackSystem playerAttackSystem;

    private int tempStrength;
    private int tempAgility;
    private int tempConstitution;
    private int tempWisdom;
    private int tempIntelligence;

    void Start()
    {
        playerHealthSystem = GetComponent<PlayerHealthSystem>();
        playerAttackSystem = GetComponent<PlayerAttackSystem>();
        ExpText = ExpBar.GetComponentInChildren<TMP_Text>();
        LevelUpPanel.SetActive(false);
        LevelUpButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Level") && !leveling)
        {
            leveling = true;
            tempStrength = Strength;
            tempAgility = Agility;
            tempConstitution = Constitution;
            tempWisdom = Wisdom;
            tempIntelligence = Intelligence;
            LevelUpPanel.SetActive(true);
            Time.timeScale = 0;
            LevelText.text = "Level : " + level.ToString();
            StartCoroutine(Leveling());
        }
        else if (Input.GetButtonDown("Level"))
        {
            Confirm();
        }

        if (leveling)return;

        ExpBar.size = ((float)exp / (float)expToLevel);
        ExpText.text = (((float)exp / (float)expToLevel) * 100).ToString("F2") + "%";

        if (exp >= expToLevel)
        {
            LevelUpButton.SetActive(true);
        }

    }

    public void AddStatPoints(int nbStatPoints)
    {
        StatPoints += nbStatPoints;
    }

    public void SubStatPoints(int nbStatPoints)
    {
        StatPoints -= nbStatPoints;
        if (StatPoints < 0)
        {
            StatPoints = 0;
        }
    }

    public void AddExp(int expToAdd)
    {
        exp += expToAdd;
    }

    public void LevelUp()
    {
        exp -= expToLevel;
        expToLevel += expToLevel * 2;
        level++;
        LevelText.text = "Level : " + level.ToString();
        leveling = true;
        AddStatPoints(5);
        tempStrength = Strength;
        tempAgility = Agility;
        tempConstitution = Constitution;
        tempWisdom = Wisdom;
        tempIntelligence = Intelligence;
        LevelUpPanel.SetActive(true);
        LevelUpButton.SetActive(false);
        Time.timeScale = 0;
        StartCoroutine(Leveling());
    }

    public void IncreaseStrength()
    {
        if (StatPoints > 0)
        {
            Strength++;
            SubStatPoints(1);
        }
    }

    public void DecreaseStrength()
    {
        
        if (Strength <= tempStrength)
        {
            Strength = tempStrength;
            return;
        }
        Strength--;
        AddStatPoints(1);
    }

    public void IncreaseAgility()
    {
        if (StatPoints > 0)
        {
            Agility++;
            SubStatPoints(1);
        }
    }

    public void DecreaseAgility()
    {
        if (Agility <= tempAgility)
        {
            Agility = tempAgility;
            return;
        }
        Agility--;
        AddStatPoints(1);
    }

    public void IncreaseConstitution()
    {
        if (StatPoints > 0)
        {
            Constitution++;
            SubStatPoints(1);
        }
    }

    public void DecreaseConstitution()
    {
        if (Constitution <= tempConstitution)
        {
            Constitution = tempConstitution;
            return;
        }
        Constitution--;
        AddStatPoints(1);
    }

    public void IncreaseWisdom()
    {
        if (StatPoints > 0)
        {
            Wisdom++;
            SubStatPoints(1);
        }
    }

    public void DecreaseWisdom()
    {
        if (Wisdom <= tempWisdom)
        {
            Wisdom = tempWisdom;
            return;
        }
        Wisdom--;
        AddStatPoints(1);
    }

    public void IncreaseIntelligence()
    {
        if (StatPoints > 0)
        {
            Intelligence++;
            SubStatPoints(1);
        }
    }

    public void DecreaseIntelligence()
    {
        if (Intelligence <= tempIntelligence)
        {
            Intelligence = tempIntelligence;
            return;
        }

        Intelligence--;
        AddStatPoints(1);
    }

    public IEnumerator Leveling()
    {
        ExpBar.gameObject.SetActive(false);
        playerHealthSystem.HealthBar.gameObject.SetActive(false);
        playerAttackSystem.ManaBar.gameObject.SetActive(false);
        playerAttackSystem.StaminaBar.gameObject.SetActive(false);
        while (leveling)
        {
            StrengthText.text = "Strength : " + Strength.ToString();
            AgilityText.text = "Agility : " + Agility.ToString();
            ConstitutionText.text = "Constitution : " + Constitution.ToString();
            WisdomText.text = "Wisdom : " + Wisdom.ToString();
            IntelligenceText.text = "Intelligence : " + Intelligence.ToString();
            StatPointsText.text = "Stat Points : " + StatPoints.ToString();
            LevelText.text = "Level : " + level.ToString();
            yield return null;
        }
        ExpBar.gameObject.SetActive(true);
        playerHealthSystem.HealthBar.gameObject.SetActive(true);
        playerAttackSystem.ManaBar.gameObject.SetActive(true);
        playerAttackSystem.StaminaBar.gameObject.SetActive(true);
        Time.timeScale = 1;
        playerHealthSystem.UpdateHealth();
        playerHealthSystem.Heal(playerHealthSystem.maxHealth);
        playerAttackSystem.UpdateStamina();
        playerAttackSystem.RefillStamina(playerAttackSystem.maxStamina);
        playerAttackSystem.UpdateMana();
        playerAttackSystem.RefillMana(playerAttackSystem.maxMana);
    }

    public void Confirm()
    {
        leveling = false;
        LevelUpPanel.SetActive(false);
    }
}
