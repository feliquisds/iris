using Platformer.Core;
using System.Collections;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
public class PlatformFall : MonoBehaviour
{
    internal Rigidbody2D rigid;
    internal CapsuleCollider2D trigger;
    PlatformerModel model = Simulation.GetModel<PlatformerModel>();
    internal float spawnX;
    internal float spawnY;
    internal float spawnZ;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = this.GetComponent<Rigidbody2D>();
        trigger = this.GetComponent<CapsuleCollider2D>();
        spawnX = this.transform.position.x;
        spawnY = this.transform.position.y;
        spawnZ = this.transform.position.z;
    }

    void OnTriggerEnter2D(Collider2D collider)
        {
            var player = model.player;
            if (collider.gameObject.tag == "Player")
            {
                player.onFallingPlat = true;
                rigid.bodyType = RigidbodyType2D.Dynamic;
                rigid.gravityScale = 0.3f;
                StartCoroutine(GoBack());
            }
        }

    IEnumerator GoBack()
    {
        yield return new WaitForSeconds(2.1f);
        this.transform.position = new Vector3(spawnX, spawnY, spawnZ);
        rigid.gravityScale = 0;
        rigid.velocity = new Vector2(0f, 0f);
        rigid.bodyType = RigidbodyType2D.Static;
    }
}
}