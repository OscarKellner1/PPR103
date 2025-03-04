public readonly struct InteractionInfo
{
    readonly InteractionPoint point;
    readonly bool hasPoint;

    public InteractionInfo(InteractionPoint point)
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

    public static InteractionInfo None()
    {
        return new InteractionInfo(null);
    }

    public bool HasPoint() { return hasPoint; }

    public InteractionPoint GetPoint() { return point; }

    public readonly bool TryGetPoint(out InteractionPoint point)
    {
        point = this.point;
        return this.hasPoint;
    }
}
