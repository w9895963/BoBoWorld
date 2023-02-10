

public interface IGetter<T>
{
    T Get();
}
public interface IGetter<P,T>
{
    T Get(P parm);
}
public interface IGetter<P1,P2,T>
{
    T Get(P1 parm1,P2 parm2);
}
public interface IGetter<P1,P2,P3,T>
{
    T Get(P1 parm1,P2 parm2,P3 parm3);
}
public interface IGetter<P1,P2,P3,P4,T>
{
    T Get(P1 parm1,P2 parm2,P3 parm3,P4 parm4);
}