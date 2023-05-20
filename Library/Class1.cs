using System;

namespace Library
{
    public class A
    {
        private int property1 { get; set; }
        public virtual int property2 { get; set; }
        public void Method1() { }
        protected void Method2() { }
        public virtual void Method3() { }
    }
    public class B : A
    {
        public override int property2 { get; set; }
        public int property3 { get; set; }
        private int property4 { get; set; }
        private int property5 { get; set; }
        public override void Method3() { }
        private void Method4() { }
        private void Method5() { }
    }
    public class C : A
    {
        public int property6 { get; set; }
        private int property7 { get; set; }
        private void Method6() { }
    }
    public class D : A
    {
        public int property8 { get; set; }
        private int property9 { get; set; }
        public void Method7() { }
    }
    public class E : B
    {
        public int property10 { get; set; }
        private int property11 { get; set; }
        public void Method8() { }
    }
}
