using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DigitalRuby.PyroParticles
{
    /// <summary>
    /// Meteor collision delegate
    /// </summary>
    /// <param name="script">Meteor swarm script</param>
    /// <param name="meteor">Meteor</param>
    public delegate void MeteorSwarmCollisionDelegate(MeteorSwarmScript script, GameObject meteor);

    /// <summary>
    /// Handles the meteor swarm effect
    /// </summary>
    public class MeteorSwarmScript : FireBaseScript, ICollisionHandler
    {
        [Tooltip("The game object prefab that represents the meteor.")]
        public GameObject MeteorPrefab;

        [Tooltip("Explosion particle system that should be emitted for each initial collision.")]
        public ParticleSystem MeteorExplosionParticleSystem;

        [Tooltip("Shrapnel particle system that should be emitted for each initial collision.")]
        public ParticleSystem MeteorShrapnelParticleSystem;

        [Tooltip("A list of materials to use for the meteors. One will be chosen at random for each meteor.")]
        public Material[] MeteorMaterials;

        [Tooltip("A list of meshes to use for the meteors. One will be chosen at random for each meteor.")]
        public Mesh[] MeteorMeshes;

        [Tooltip("The destination radius")]
        public float DestinationRadius;

        [Tooltip("The source of the meteor swarm (in the sky somewhere usually)")]
        public Vector3 Source;

        [Tooltip("The source radius")]
        public float SourceRadius;

        [Tooltip("The time it should take the meteors to impact assuming a clear path to destination.")]
        public float TimeToImpact = 1.0f;

        [SingleLine("How many meteors should be emitted per second (min and max)")]
        public RangeOfIntegers MeteorsPerSecondRange = new RangeOfIntegers { Minimum = 5, Maximum = 10 };

        [SingleLine("Scale multiplier for meteors (min and max)")]
        public RangeOfFloats ScaleRange = new RangeOfFloats { Minimum = 0.25f, Maximum = 1.5f };

        [SingleLine("Maximum life time of meteors in seconds (min and max).")]
        public RangeOfFloats MeteorLifeTimeRange = new RangeOfFloats { Minimum = 4.0f, Maximum = 8.0f };

        [Tooltip("Array of emission sounds. One will be chosen at random upon meteor creation.")]
        public AudioClip[] EmissionSounds;

        [Tooltip("Array of explosion sounds. One will be chosen at random upon impact.")]
        public AudioClip[] ExplosionSounds;

        /// <summary>
        /// A delegate that can be assigned to listen for collision. Use this to apply damage for meteor impacts or other effects.
        /// </summary>
        [HideInInspector]
        public event MeteorSwarmCollisionDelegate CollisionDelegate;

        private float elapsedSecond = 1.0f;

		private float scale = .50f;
		private float initScale = .50f;
		
		private float[] delays = { .17f, .84f, .45f, .32f, .68f, .74f, .43f };
		private int delayCount = 0;
		
		private Vector3[] insideUnitSphereRandoms = { new Vector3(-.2f,.1f,.9f),
													  new Vector3(-.3f,.3f,.2f),
													  new Vector3(-.9f,.1f,.1f),
													  new Vector3(-.2f,.5f,-.2f),
													  new Vector3(0f,.2f,-.2f),
													  new Vector3(-.4f,.1f,-.8f),
													  new Vector3(.1f,.5f,-.1f),
													  new Vector3(.4f,.1f,-.4f),
													  new Vector3(-.5f,.4f,-.3f),
													  new Vector3(.7f,.1f,-.6f),
													  new Vector3(-.2f,.6f,-.9f),
													  new Vector3(.8f,-.6f,.1f),
													  new Vector3(-.3f,-.2f,-.9f),
													  new Vector3(.8f,.5f,.1f),
													  new Vector3(0f,.2f,-.2f),
													  new Vector3(-.9f,.9f,-.3f),
													  new Vector3(.8f,-.3f,-.7f),
													  new Vector3(.4f,-.1f,-.4f),
													  new Vector3(.7f,-.7f,-.5f),
													  new Vector3(.1f,.2f,.3f),
													  new Vector3(-.3f,-.4f,-.1f),
													  new Vector3(-.7f,.5f,.3f),
													  new Vector3(.3f,.7f,-.9f),
													  new Vector3(.2f,-.2f,0f),
													  new Vector3(.1f,0f,-.7f),
													  new Vector3(.1f,-.1f,-.6f),
													  new Vector3(-.2f,-.8f,.5f)};
													  
		private int insideCounter = 0;
		private int insideCounter2 = 22/2; //halfway through the list 
		
        private IEnumerator SpawnMeteor()
        {
            {
				if (delayCount >= delays.Length) {delayCount = 0;}
                float delay = delays[delayCount++];//UnityEngine.Random.Range(0.0f, 1.0f);
				//float delay = UnityEngine.Random.Range(0.0f, 1.0f);
                yield return new WaitForSeconds(delay);
            }

            // find a random source and destination point within the specified radius
			
			if (insideCounter >= insideUnitSphereRandoms.Length) {insideCounter = 0;}
			if (insideCounter2 >= insideUnitSphereRandoms.Length) {insideCounter2 = 0;}
			//Vector3 temp = ;//UnityEngine.Random.insideUnitSphere;
			//Debug.Log(temp);
			
            Vector3 src = Source + ( insideUnitSphereRandoms[insideCounter++] * SourceRadius);
			
            GameObject meteor = GameObject.Instantiate(MeteorPrefab);
            if(scale>initScale*4){scale = scale / 4.0f;}else{scale+=initScale;} //float scale = UnityEngine.Random.Range(ScaleRange.Minimum, ScaleRange.Maximum);
            meteor.transform.localScale = new Vector3(scale, scale, scale);
            meteor.transform.position = src;
            Vector3 dest = gameObject.transform.position + (insideUnitSphereRandoms[insideCounter2++] * DestinationRadius);
			dest.y = 0.0f;

            // get the direction and set speed based on how fast the meteor should arrive at the destination
            Vector3 dir = (dest - src);
            Vector3 vel = dir / TimeToImpact;
            Rigidbody r = meteor.GetComponent<Rigidbody>();
            r.velocity = vel;
            float xRot = 37f;//UnityEngine.Random.Range(-90.0f, 90.0f);
            float yRot = 37f;//UnityEngine.Random.Range(-90.0f, 90.0f);
            float zRot = 37f;//UnityEngine.Random.Range(-90.0f, 90.0f);
            r.angularVelocity = new Vector3(xRot, yRot, zRot);
            r.mass *= (scale * scale);

            // setup material
            Renderer renderer = meteor.GetComponent<Renderer>();
            renderer.sharedMaterial = MeteorMaterials[UnityEngine.Random.Range(0, MeteorMaterials.Length)];
            meteor.transform.parent = gameObject.transform;
            meteor.GetComponent<FireCollisionForwardScript>().CollisionHandler = this;

            // setup mesh
            Mesh mesh = MeteorMeshes[UnityEngine.Random.Range(0, MeteorMeshes.Length - 1)];
            meteor.GetComponent<MeshFilter>().mesh = mesh;

            // setup trail
            TrailRenderer t = meteor.GetComponent<TrailRenderer>();
            t.startWidth = 2.5f * scale;//UnityEngine.Random.Range(2.0f, 3.0f) * scale;
            t.endWidth = .4f * scale;//UnityEngine.Random.Range(0.25f, 0.5f) * scale;
            t.time = .33f;//UnityEngine.Random.Range(0.25f, 0.5f);

            // play sound
            if (EmissionSounds != null && EmissionSounds.Length != 0)
            {
                AudioSource audio = meteor.GetComponent<AudioSource>();
                if (audio != null)
                {
                    int index = UnityEngine.Random.Range(0, EmissionSounds.Length);
                    AudioClip clip = EmissionSounds[index];
                    audio.PlayOneShot(clip, scale);
                }
            }
        }

        private void SpawnMeteors()
        {
            int count = (int)UnityEngine.Random.Range(MeteorsPerSecondRange.Minimum, MeteorsPerSecondRange.Maximum);
            for (int i = 0; i < count; i++)
            {
                StartCoroutine(SpawnMeteor());
            }
        }

        protected override void Update()
        {
 	        base.Update();

            if (Duration > 0.0f && (elapsedSecond += Time.deltaTime) >= 1.0f)
            {
                elapsedSecond = elapsedSecond - 1.0f;
                SpawnMeteors();
            }
        }

        private void PlayCollisionSound(GameObject obj)
        {
            if (ExplosionSounds == null || ExplosionSounds.Length == 0)
            {
                return;
            }

            AudioSource s = obj.GetComponent<AudioSource>();
            if (s == null)
            {
                return;
            }

            int index = UnityEngine.Random.Range(0, ExplosionSounds.Length);
            AudioClip clip = ExplosionSounds[index];
            s.PlayOneShot(clip, obj.transform.localScale.x);
        }

        private IEnumerator CleanupMeteor(float delay, GameObject obj)
        {
            yield return new WaitForSeconds(delay);

            GameObject.Destroy(obj.GetComponent<Collider>());
            GameObject.Destroy(obj.GetComponent<Rigidbody>());
            GameObject.Destroy(obj.GetComponent<TrailRenderer>());
        }

        public void HandleCollision(GameObject obj, Collision col)
        {
            Renderer r = obj.GetComponent<Renderer>();
            if (r == null)
            {
                return;
            }
            else if (CollisionDelegate != null)
            {
                CollisionDelegate(this, obj);
            }

            Vector3 pos, normal;
            if (col.contacts.Length == 0)
            {
                pos = obj.transform.position;
                normal = -pos;
            }
            else
            {
                pos = col.contacts[0].point;
                normal = col.contacts[0].normal;
            }

            MeteorExplosionParticleSystem.transform.position = pos;
            MeteorExplosionParticleSystem.transform.rotation = Quaternion.LookRotation(normal);
            MeteorExplosionParticleSystem.Emit(UnityEngine.Random.Range(10, 20));
            MeteorShrapnelParticleSystem.transform.position = col.contacts[0].point;
            MeteorShrapnelParticleSystem.Emit(UnityEngine.Random.Range(10, 20));

            PlayCollisionSound(obj);

            GameObject.Destroy(r);

            StartCoroutine(CleanupMeteor(0.1f, obj));
            GameObject.Destroy(obj, 4.0f);
        }
    }
}
