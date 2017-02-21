using UnityEngine;
using System.Collections;

public class EntityBase: MonoBehaviour {
    public int Id { set; get; }
    public ObjectType Type { set; get; }

    public EntityBase(int id) { Id = id; }
}
