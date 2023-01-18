using Android.Views;
using Android.Views.Animations;

namespace StajBilgisi
{
    class MyAnimation : Animation
    {
        private View mView;
        private int mOriginalHeight; //Animasyon başlamadan önceki varsayılan yükseklik için bunu ayarlıyoruz !
        private int mTargetHeight;//Büyümenin ne şekilde olacağı ! Yüksekliğin hangi integer değere gideceği
        private int mGrowBy; //Nasıl büyüyeceği yada shrink diyebiliriz . 

        public MyAnimation(View view, int targetHeight)
        {
            mView = view;
            mOriginalHeight = view.Height;
            mTargetHeight = targetHeight;
            mGrowBy = mTargetHeight - mOriginalHeight; //burada ne kadar büyümesi gerektiğini hesaplıyoruz ! 
        }
        //Bunları yapmanın birçok yolu avr ben bu yolunu öğrendim ! 
        protected override void ApplyTransformation(float interpolatedTime, Transformation t)
        {
            mView.LayoutParameters.Height = (int)(mOriginalHeight + (mGrowBy * interpolatedTime)); //Bu *2 *3 şeklinde büyümesini sağlıyor !
            mView.RequestLayout(); //Bu override method çok onemli kendini birden fazla kez recursive olarak çağırıyor ! 
        }
        public override bool WillChangeBounds()
        {
            return true;
        }
    }
}