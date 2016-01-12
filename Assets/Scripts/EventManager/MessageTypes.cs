using UnityEngine;
using System.Collections;

//Add one of these for each type of message

namespace ExtendedEvents
{
    //Base Class for all Message Info. Override for message types
    public class MessageInfo { }

    public class InitSpawnersMessage : MessageInfo
    {
        public InitSpawnersMessage()
        {
            //Do nothing in constructor
        }
    }

    public class LaserHitMessage : MessageInfo
    {
        public int ID { get; private set; }
        public Collision2D collision { get; private set; }

        public LaserHitMessage(int id, Collision2D collision)
            {
                this.ID = id;
                this.collision = collision;
            }
    }

    public class ChargeHitMessage : MessageInfo
    {
        public float cameraShakeTime { get; private set; }

        public ChargeHitMessage(float shakeTime)
        {
            cameraShakeTime = shakeTime;
        }
    }

    public class ShakeCameraMessage : MessageInfo
    {
        public float shakeTime { get; private set; }

        public ShakeCameraMessage(float time)
        {
            Debug.Log("Made a new shake message");
            shakeTime = time;
        }
    }

    public class PlayerRespawnedMessage : MessageInfo
    {
        public Vector3 target { get; private set; }

        public PlayerRespawnedMessage(Vector3 target)
        {
            this.target = target;
        }
    }

    public class EnemyDamagedMessage : MessageInfo
    {
        public float damage { get; private set; }

        public EnemyDamagedMessage(float damage = 0f)
        {
            this.damage = damage;
        }
    }

    public class EnemyDiedMessage : MessageInfo
    {
        public EnemyDiedMessage()
        {
            //Do nothing (for now atleast)
        }
    }

    public class PowerChangeMessage : MessageInfo
    {
        public PowerManager.PowerName associatedPower;
        public Power.PowerUsageMode usageModeOfSlot;

        public PowerChangeMessage(PowerManager.PowerName power, Power.PowerUsageMode usageMode)
        {
            associatedPower = power;
            usageModeOfSlot = usageMode;
        }
    }


    public class ApplyDamageMessage : MessageInfo
    {
        public int entityID;
        public float damage;
        public float duration;

        public ApplyDamageMessage(int ID, float damage, float duration)
        {
            this.entityID = ID;
            this.damage = damage;
            this.duration = duration;
        }
    }
}
