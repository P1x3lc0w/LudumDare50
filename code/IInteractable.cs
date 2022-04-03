using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;


public interface IInteractable
{
    Vector2 GlobalPosition {get;}
    void Focus();
    void Blur();
    bool BeginInteract();
    bool Update(float delta);
}
