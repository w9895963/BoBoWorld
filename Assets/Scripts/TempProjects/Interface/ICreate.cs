

public interface ICreate<T>
{
    T Create();
}
public interface ICreate<P, T>
{
    T Create(P parm);
}
public interface ICreate<P1, P2, T>
{
    T Create(P1 parm1, P2 parm2);
}
public interface ICreate<P1, P2, P3, T>
{
    T Create(P1 parm1, P2 parm2, P3 parm3);
}
public interface ICreate<P1, P2, P3, P4, T>
{
    T Create(P1 parm1, P2 parm2, P3 parm3, P4 parm4);
}