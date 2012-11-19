package javasourcesplashscreen;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.view.Window;
import android.view.WindowManager;
import com.itrmobility.customermanagement.R;

public class SplashScreenActivity extends Activity {

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

		android.util.Log.i("SplashScreenActivity", "onCreate");

        requestWindowFeature(Window.FEATURE_NO_TITLE);
        getWindow().setFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN, WindowManager.LayoutParams.FLAG_FULLSCREEN);

        setContentView(R.layout.splash);
        splashHandler.sendMessageDelayed(Message.obtain(splashHandler, 0), 500);
    }

    @Override
    public void onBackPressed() {
        //Do nothing here. We just want to block the back button on the splash screen.
    }

    private Handler splashHandler = new Handler() {
        @Override
        public void handleMessage(Message msg) {
			android.util.Log.i("SplashScreenActivity", "sending Broadcast");
            sendBroadcast(new Intent("MonoCross.MainReceiver.CustomerManagement"));
            super.handleMessage(msg);
			//finish();
        }
    };
}