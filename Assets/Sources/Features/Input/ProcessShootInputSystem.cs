﻿using Entitas;
using System.Collections.Generic;
using UnityEngine;

public class ProcessShootInputSystem : IReactiveSystem, ISetPool {

    public TriggerOnEvent trigger { get { return InputMatcher.ShootInput.OnEntityAdded(); } }

    Pool _inputPool;
    readonly Group _players;
    readonly Pool _bulletsPool;
    readonly ObjectPool<GameObject> _bulletsObjectPool;

    public void SetPool(Pool pool) {
        _inputPool = pool;
    }

    public ProcessShootInputSystem(Pool corePool, Pool bulletsPool) {
        _players = corePool.GetGroup(Matcher.AllOf(CoreMatcher.Player, CoreMatcher.Position));
        _bulletsPool = bulletsPool;
        _bulletsObjectPool = new ObjectPool<GameObject>(() => Object.Instantiate(Resources.Load<GameObject>(Res.Bullet)));
    }

    public void Execute(List<Entity> entities) {
        var input = entities[entities.Count - 1];
        var ownerId = input.inputOwner.playerId;


        foreach (var e in _players.GetEntities()) {
            if (e.player.id == ownerId) {
                // TODO Remove, just for testing
                // var velocity = new Vector3(-0.05f + Random.value * 0.1f, 0.5f, 0);
                var velocity = new Vector3(0, 0.5f, 0);
                _bulletsPool.CreateBullet(e.position.value, velocity, _bulletsObjectPool);
            }
        }

        _inputPool.DestroyEntity(input);
    }
}
