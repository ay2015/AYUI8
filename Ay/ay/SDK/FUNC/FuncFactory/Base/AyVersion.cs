namespace ay.FuncFactory.Base
{
    /// <summary>
    /// ay用于对比版本
    /// 2015-8-3 10:46:39
    /// </summary>
    public struct AyVersion
    {
        public int a, b, c, d;
        public AyVersion(int a, int b, int c, int d)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
        }

        public AyVersion(AyVersion av)
        {
            this.a = av.a;
            this.b = av.b;
            this.c = av.c;
            this.d = av.d;
        }
        public override string ToString()
        {
            return "(" + a + "." + b + "." + c + "." + d + ")";
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(AyVersion lav, AyVersion rav)
        {
            if (lav.a == rav.a && lav.b == rav.b && lav.c == rav.c && lav.d == rav.d)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool operator !=(AyVersion lav, AyVersion rav)
        {
            return !(lav == rav);
        }

        public static bool operator >(AyVersion lav, AyVersion rav)
        {
            if (lav.a > rav.a)
            {
                return true;
            }
            else if (lav.a == rav.a)
            {
                if (lav.b > rav.b)
                {
                    return true;
                }
                else if (lav.b == rav.b)
                {
                    if (lav.c > rav.c)
                    {
                        return true;
                    }
                    else if (lav.c == rav.c)
                    {
                        if (lav.d > rav.d)
                        {
                            return true;
                        }
                        else if (lav.d == rav.d)
                        {
                            return false;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }



        }
        public static bool operator <(AyVersion lav, AyVersion rav)
        {
            return !(lav > rav);
        }

    }
}
