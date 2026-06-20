namespace SVI_NFT_R
{
    public static partial class Utils
    {
        public sealed class ConditionBuilder
        {
            public bool IsInitialized => bCurrentCondition != null;
            public bool? Result => bCurrentCondition;
            private bool? bCurrentCondition = null;

            public static bool operator ==(bool left, ConditionBuilder right)
            {
                return left == right.Result;
            }

            public static bool operator ==(ConditionBuilder left, bool right)
            {
                return left.Result == right;
            }

            public static bool operator ==(ConditionBuilder left, ConditionBuilder right)
            {
                return left.Result == right.Result;
            }

            public static bool operator !=(bool left, ConditionBuilder right)
            {
                return left != right.Result;
            }

            public static bool operator !=(ConditionBuilder left, bool right)
            {
                return left.Result != right;
            }

            public static bool operator !=(ConditionBuilder left, ConditionBuilder right)
            {
                return left.Result != right.Result;
            }

            public ConditionBuilder(bool? initialConditionOrNull = null)
            {
                bCurrentCondition = initialConditionOrNull;
            }

            public ConditionBuilder Reset(bool? initialConditionOrNull = null)
            {
                bCurrentCondition = initialConditionOrNull;
                return this;
            }

            public ConditionBuilder And(bool condition)
            {
                if (bCurrentCondition.HasValue == true)
                {
                    bCurrentCondition &= condition;
                }
                else
                {
                    bCurrentCondition = condition;
                }
                return this;
            }

            public ConditionBuilder Or(bool condition)
            {
                if (bCurrentCondition.HasValue == true)
                {
                    bCurrentCondition |= condition;
                }
                else
                {
                    bCurrentCondition = condition;
                }
                return this;
            }

            public override bool Equals(object obj)
            {
                if (IsInitialized == false)
                {
                    return false;
                }
                if (obj is ConditionBuilder)
                {
                    var compareObject = obj as ConditionBuilder;
                    return compareObject.Result == bCurrentCondition;
                }
                else if (obj is bool)
                {
                    var compareObject = (bool)obj;
                    return compareObject == bCurrentCondition;
                }
                return false;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }
    }

}
