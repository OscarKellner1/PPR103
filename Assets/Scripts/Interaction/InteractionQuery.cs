public readonly struct InteractionQuery
{
    readonly InteractionObject point;
    readonly bool hasPoint;

    public InteractionQuery(InteractionObject point)
    {
        this.point = point;
        if (point == null )
        {
            this.hasPoint = false;
        }
        else
        {
            this.hasPoint = true;
        }
    }

    public static InteractionQuery None()
    {
        return new InteractionQuery(null);
    }

    public bool HasObject() { return hasPoint; }

    public InteractionObject GetObject() { return point; }

    public readonly bool TryGetObject(out InteractionObject point)
    {
        point = this.point;
        return this.hasPoint;
    }
}
