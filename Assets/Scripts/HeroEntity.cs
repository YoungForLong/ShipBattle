using UnityEngine;
using System.Collections;

public class HeroEntity : EntityBase {

    public HeroEntity(int id) : base(id) { }

    private void Awake()
    {
        //test
        Type = EntityType.ship;

        this._id = CommonEnum.hero_id;
        EntityMgr.Instance().RegisterEntity(this);
    }
}
