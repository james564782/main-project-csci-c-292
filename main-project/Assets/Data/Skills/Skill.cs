using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkillData", menuName = "Skills/Skill Data")]
public class Skill : ScriptableObject
{
    [SerializeField] SkillEvent[] events;
    [SerializeField] int sP;
    [SerializeField] string description;
    [SerializeField] AudioClip sound;

    public int GetSPCost() {
        return sP;
    }
    public string GetDescription() {
        return description;
    }

    public AudioClip GetSound() {
        return sound;
    }

    public void PlaySound() {
        SoundBoard.instance.PlayAudio(sound);
    }
    public void PlaySound(float volumeScale) {
        SoundBoard.instance.PlayAudio(sound, volumeScale);
    }

    public SkillEvent[] GetEvents() {
        return events;
    }
    public SkillEvent GetEvent(int index) {
        if (index < events.Length) {
            return events[index];
        }
        Debug.Log("Something went wrong in GetEvent Skill");
        return null;
    }
}
