using UnityEngine;
using System.Collections;

/**
  * Scene:All
  * Object:SoundManager
  * Description: Skripta zaduzena za zvuke u apliakciji, njihovo pustanje, gasenje itd...
  **/
public class SoundManager : MonoBehaviour {

	public static int musicOn = 1;
	public static int soundOn = 1;
	public static bool forceTurnOff = false;


	public AudioSource menuMusic;
	public AudioSource gameplayMusic;
	public AudioSource buttonClick;
	public AudioSource Ingredient;
	public AudioSource WrongIngredient;
	public AudioSource CustomerArrival;
	public AudioSource CustomerHappyDeparting_female;
	public AudioSource CustomerHappyDeparting;
	public AudioSource UnlockNewItem;
	public AudioSource Coins;
	public AudioSource NoMoney;
	public AudioSource Win;
	public AudioSource SpecialOffer;
	public AudioSource LockedItem;
	public AudioSource EggBreak;
	public AudioSource LiquidIngredient;
	public AudioSource PowderIngredient;
	public AudioSource MixIngredients;
	public AudioSource Timer;
	public AudioSource Lose;
	public AudioSource OnOffSound;
	public AudioSource WireSparks;



	static SoundManager instance;

	public static SoundManager Instance
	{
		get
		{
			if(instance == null)
			{
				instance = GameObject.FindObjectOfType(typeof(SoundManager)) as SoundManager;
			}

			return instance;
		}
	}

	void Awake () 
	{

		if(instance != null && instance !=this)
			GameObject.DestroyImmediate(this.gameObject);
	}

	void Start () 
	{
		DontDestroyOnLoad(this.gameObject);

		//		if(PlayerPrefs.HasKey("SoundOn"))PlayerPrefs.GetInt("SoundOn"
//		{
			musicOn = PlayerPrefs.GetInt("MusicOn",1);
			soundOn = PlayerPrefs.GetInt("SoundOn",1);
		//}

		Screen.sleepTimeout = SleepTimeout.NeverSleep; 
	}

	public void Play_ButtonClick()
	{
		if(buttonClick.clip != null && soundOn == 1)
			buttonClick.Play();
	}

	public void Play_MenuMusic()
	{
		if(menuMusic.clip != null && musicOn == 1 && !menuMusic.isPlaying)
			menuMusic.Play();
	}

	public void Stop_MenuMusic()
	{
		if(menuMusic.clip != null && musicOn == 1)
			menuMusic.Stop();
	}

	public void Play_GameplayMusic()
	{
		if(gameplayMusic.clip != null && musicOn == 1)
		{
			gameplayMusic.Play();
		}
	}

	public void Stop_GameplayMusic()
	{
		if(gameplayMusic.clip != null && musicOn == 1)
		{
			StartCoroutine(FadeOut(gameplayMusic, 0.1f));
		}
	}

	/// <summary>
	/// Corutine-a koja za odredjeni AudioSource, kroz prosledjeno vreme, utisava AudioSource do 0, gasi taj AudioSource, a zatim vraca pocetni Volume na pocetan kako bi AudioSource mogao opet da se koristi
	/// </summary>
	/// <param name="sound">AudioSource koji treba smanjiti/param>
	/// <param name="time">Vreme za koje treba smanjiti Volume/param>
	IEnumerator FadeOut(AudioSource sound, float time)
	{
		float originalVolume = sound.volume;
		while(sound.volume != 0)
		{
			sound.volume = Mathf.MoveTowards(sound.volume, 0, time);
			yield return null;
		}
		sound.Stop();
		sound.volume = originalVolume;
	}

	public void	Play_Sound(AudioSource sound)
	{
		if(!sound.isPlaying  && soundOn == 1) 
			sound.Play();
	}

	public void	Stop_Sound(AudioSource sound)
	{
 
		if(sound.isPlaying)
			sound.Stop();
	}

	public void	PlayWin_Sound( )
	{
		if(  soundOn == 1) 
			StartCoroutine("WinSoundWithMenuMusicFade");

	}

	IEnumerator WinSoundWithMenuMusicFade( )
	{
		float time = 1f;
		float originalVolume = menuMusic.volume;
		while(menuMusic.volume != 0.1f)
		{
			menuMusic.volume = Mathf.MoveTowards(menuMusic.volume, 0.1f, time);
			yield return null;
		}

		Win.Play();
		yield return new WaitForSeconds(2.0f);
		Win.Stop();
		while(menuMusic.volume != originalVolume)
		{
			menuMusic.volume = Mathf.MoveTowards(0.1f ,originalVolume,   time);
			yield return null;
		}
		 
		menuMusic.volume = originalVolume;
	}

	
}
