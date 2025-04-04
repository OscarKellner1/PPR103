public enum InteractibleType
{
    None = 0,
    InteractionObject = 1,
    PickupObject = 2,
}

public readonly struct InteractionQuery
{
    readonly InteractionObject interactionObject;
    readonly PickUpObject pickUpObject;

    public InteractionQuery(InteractionObject obj)
    {
        this.interactionObject = obj;
        this.pickUpObject = null;
    }

    public InteractionQuery(PickUpObject obj)
    {
        this.interactionObject = null;
        this.pickUpObject = obj;
    }

    public static InteractionQuery None()
    {
        return new InteractionQuery();
    }

    public bool HasObject() { return interactionObject != null || pickUpObject != null; }

    public InteractibleType ObjectType()
    {
        if (interactionObject != null)
        {
            return InteractibleType.InteractionObject;
        }
        else if (pickUpObject != null)
        {
            return InteractibleType.PickupObject;
        }
        else
        {
            return InteractibleType.None;
        }
    }

    public readonly bool TryGetInteractionObject(out InteractionObject obj)
    {
        obj = this.interactionObject;
        return obj != null;
    }

    public readonly bool TryGetPickUpObject(out PickUpObject obj)
    {
        obj = this.pickUpObject;
        return obj != null;
    }
}
