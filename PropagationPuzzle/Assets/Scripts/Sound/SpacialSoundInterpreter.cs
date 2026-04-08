using UnityEngine;

public class SpacialSoundInterpreter : MonoBehaviour
{
    [SerializeField] int directionCount;
    public GameObject Emitter;

    public SoundEmitter[] soundEmitters;

    private void Start()
    {
        soundEmitters = new SoundEmitter[directionCount];
        CreateSoundEmitters ();
    }

    public void CreateSoundEmitters ()
    {
        if (Emitter.GetComponent<SoundEmitter>() == null)
        {
            Debug.LogError("Sound Emitter Script missing");
            return;
        }

        for (int i = 0; i < directionCount; i++)
        {
            GameObject emitter = Instantiate(Emitter, transform);

            float direction = 360f / directionCount * i;
            Vector2 emitterDirection = VectorOperation.Rotate2(Vector2.up, direction);
            emitter.transform.localPosition = new Vector3(emitterDirection.x, 0f, emitterDirection.y);

            soundEmitters[i] = emitter.GetComponent<SoundEmitter>();
            soundEmitters[i].intensityMulitplier = 1 / directionCount;
        }
    }
    public void ResetEmitterValues()
    {
        foreach(SoundEmitter soundEmitter in soundEmitters)
        {
            soundEmitter.ResetValues();
        }
    }

    public void AddSoundRay (SoundRay soundRay)
    {
        //Gets angle of incomming ray
        Vector2 angleVector = new Vector2(soundRay.direction.x, soundRay.direction.z);
        float angle = Vector2.SignedAngle(Vector2.up, angleVector);
        if (angle < 0f)
        {
            angle += 360f;
        } else if (angle > 360f)
        {
            angle -= 360;
        }

        //Finds appropiate emitter
        int emitterNumber = (int) (angle / 360f * directionCount);
        soundEmitters[emitterNumber].AddRay(soundRay);

    }
   
}
