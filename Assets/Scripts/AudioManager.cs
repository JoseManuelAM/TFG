using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Enumeraci�n con las distintas m�sicas de los niveles
public enum LevelStageMusic
{
    Overworld,
    Underworld,
    Castle,
    IntoTunnel
}

public class AudioManager : MonoBehaviour
{
    //Clips de audio y SFX
    public AudioClip clipJump;
    public AudioClip clipBigJump;
    public AudioClip clipCoin;
    public AudioClip clipStomp;
    public AudioClip clipFlipDie;
    public AudioClip clipShoot;
    public AudioClip clipPowerUp;
    public AudioClip clipPowerDown;
    public AudioClip clipPowerUpAppear;
    public AudioClip clipBreak;
    public AudioClip clipBump;
    public AudioClip clipDie;
    public AudioClip clipFlagPole;
    public AudioClip clipOneUp;
    public AudioClip clipBowserFall;

    public AudioSource sfx;
    public AudioSource music;


    public AudioClip clipLevelOverworld;
    public AudioClip clipLevelUnderworld;
    public AudioClip clipLevelCastle;
    public AudioClip clipStarman;
    public AudioClip clipLevelCompleted;
    public AudioClip clipCastleComplete;
    public AudioClip clipGameOver;
    public AudioClip clipStartIntoTunnel;


    LevelStageMusic currentMusic;
    bool starmanMode;

    //Instancia para acceder al AudioManager desde otros scripts
    public static AudioManager Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    //M�todos para reproducir los distintos efectos de sonido
    public void PlayJump()
    {
        sfx.PlayOneShot(clipJump);
    }
    public void PlayBigJump()
    {
        sfx.PlayOneShot(clipBigJump);
    }
    public void PlayCoin()
    {
        sfx.PlayOneShot(clipCoin);
    }
    public void PlayStomp()
    {
        sfx.PlayOneShot(clipStomp);
    }
    public void PlayFlipDie()
    {
        sfx.PlayOneShot(clipFlipDie);
    }
    public void PlayShoot()
    {
        sfx.PlayOneShot(clipShoot);
    }
    public void PlayPowerUp()
    {
        sfx.PlayOneShot(clipPowerUp);
    }
    public void PlayPowerDown()
    {
        sfx.PlayOneShot(clipPowerDown);
    }
    public void PlayPipe()
    {
        sfx.PlayOneShot(clipPowerDown);
    }
    public void PlayPowerUpAppear()
    {
        sfx.PlayOneShot(clipPowerUpAppear);
    }
    public void PlayBreak()
    {
        sfx.PlayOneShot(clipBreak);
    }
    public void PlayBump()
    {
        sfx.PlayOneShot(clipBump);
    }
    public void PlayOneUp()
    {
        sfx.PlayOneShot(clipOneUp);
    }
    public void PlayBowserFall()
    {
        sfx.PlayOneShot(clipBowserFall);
    }


    public void PlayDie()
    {
        music.pitch = 1f;
        music.clip = clipDie;
        music.loop = false;
        music.Play();
    }
    public void PlayFlagPole()
    {
        music.pitch = 1f;
        music.clip = clipFlagPole;
        music.loop = false;
        music.Play();
    }

    //M�todos para reproducir la m�sica de fondo del nivel seg�n el nivel actual
    public void PlayLevelStageMusic(LevelStageMusic levelStageMusic)
    {
        switch(levelStageMusic)
        {
            case LevelStageMusic.Overworld:
                MusicLevelOverworld();
                break;
            case LevelStageMusic.Underworld:
                MusicLevelUnderworld();
                break;
            case LevelStageMusic.Castle:
                MusicLevelCastle();
                break;
            case LevelStageMusic.IntoTunnel:
                PlayStartIntoTunnel();
                break;
        }
    }
    //M�todos con las distintas m�sicas de cada nivel
    void MusicLevelOverworld()
    {
        currentMusic = LevelStageMusic.Overworld;
        if(!starmanMode)
        {
            music.clip = clipLevelOverworld;
            music.loop = true;
            music.Play();
        }
    }
    void MusicLevelUnderworld()
    {
        currentMusic = LevelStageMusic.Underworld;
        if(!starmanMode)
        {
            music.clip = clipLevelUnderworld;
            music.loop = true;
            music.Play();
        } 
    }
    void MusicLevelCastle()
    {
        currentMusic = LevelStageMusic.Castle;
        if(!starmanMode)
        {
            music.clip = clipLevelCastle;
            music.loop = true;
            music.Play();
        } 
    }
    void PlayStartIntoTunnel()
    {
        music.clip = clipStartIntoTunnel;
        music.loop = false;
        music.Play();
    }
    //M�todos para manejar la m�sica si el jugador recoge una estrella
    public void MusicStarman()
    {
        starmanMode = true;
        music.clip = clipStarman;
        music.loop = true;
        music.Play();
    }
    public void StopMusicStarman(bool playLevelStageMusic)
    {
        if(starmanMode)
        {
            starmanMode = false;
            if(playLevelStageMusic)
            {
                PlayLevelStageMusic(currentMusic);
            }
        }
    }

    public void PlayLevelCompleted()
    {
        music.pitch = 1f;
        music.clip = clipLevelCompleted;
        music.loop = false;
        music.Play();
    }
    public void PlayCastleCompleted()
    {
        music.pitch = 1f;
        music.clip = clipCastleComplete;
        music.loop = false;
        music.Play();
    }
    public void PlayGameOver()
    {
        music.pitch = 1f;
        music.clip = clipGameOver;
        music.loop = false;
        music.Play();
    }
    //M�todo para acelerar la m�sica
    public void HurryUp()
    {
        music.pitch = 1.5f;
    }
}
