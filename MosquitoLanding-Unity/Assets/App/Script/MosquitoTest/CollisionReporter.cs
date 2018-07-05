using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionReporter : MonoBehaviour {
    BaseCharacter _baseCharacter;
    public bool isBreakable = false;

    void Start() {
        _baseCharacter = GetComponentInParent<BaseCharacter>();
    }

    public bool IsBreakUp() {
        return (_baseCharacter.transform.Find(transform.name) == null);
    }

    //Break from parent.tranfrom, to outer layer
    public void SeperateFromMainBody() {
        transform.SetParent(_baseCharacter.transform.parent);

        Rigidbody2D itemRigid = GetComponent<Rigidbody2D>();
        if (!itemRigid) gameObject.AddComponent<Rigidbody2D>();

        Collider2D boxCollider =  GetComponent<Collider2D>();
        if (boxCollider) boxCollider.enabled = true;
    }

	void OnCollisionEnter2D(Collision2D collision) {
        if (IsBreakUp()) return;

        _baseCharacter.OnCollisionHandler(collision, transform);
    }

}
