package com.example.smtp;

import android.Manifest;
import android.content.pm.PackageManager;
import android.os.AsyncTask;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;
import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.app.ActivityCompat;
import androidx.core.content.ContextCompat;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;
import jakarta.mail.Message;
import jakarta.mail.MessagingException;
import jakarta.mail.PasswordAuthentication;
import jakarta.mail.Session;
import jakarta.mail.Transport;
import jakarta.mail.internet.InternetAddress;
import jakarta.mail.internet.MimeMessage;
import java.util.Properties;

public class MainActivity extends AppCompatActivity {

    private static final int INTERNET_PERMISSION_REQUEST_CODE = 1001;
    private EditText editTextTo;
    private EditText editTextSubject;
    private EditText editTextMessage;
    private Button buttonSend;


    private static final String SENDER_EMAIL = "alex_pak2006@mail.ru";
    private static final String SENDER_PASSWORD = "nSr9tzcazKTXzTyqJuki"; // Пароль приложения Mail.ru
    private static final String TAG = "SendEmail";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_main);
        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });


        editTextTo = findViewById(R.id.editTextTo);
        editTextSubject = findViewById(R.id.editTextSubject);
        editTextMessage = findViewById(R.id.editTextMessage);
        buttonSend = findViewById(R.id.buttonSend);


        if (ContextCompat.checkSelfPermission(this, Manifest.permission.INTERNET) != PackageManager.PERMISSION_GRANTED) {
            ActivityCompat.requestPermissions(this, new String[]{Manifest.permission.INTERNET}, INTERNET_PERMISSION_REQUEST_CODE);
        }


        buttonSend.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                String recipient = editTextTo.getText().toString().trim();
                String subject = editTextSubject.getText().toString().trim();
                String message = editTextMessage.getText().toString().trim();


                if (recipient.isEmpty() || subject.isEmpty() || message.isEmpty()) {
                    Toast.makeText(MainActivity.this, "Заполните все поля", Toast.LENGTH_SHORT).show();
                    return;
                }


                new SendEmailTask().execute(recipient, subject, message);
            }
        });
    }

    @Override
    public void onRequestPermissionsResult(int requestCode, String[] permissions, int[] grantResults) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults);
        if (requestCode == INTERNET_PERMISSION_REQUEST_CODE) {
            if (grantResults.length > 0 && grantResults[0] == PackageManager.PERMISSION_GRANTED) {
                Toast.makeText(this, "Разрешение на интернет предоставлено", Toast.LENGTH_SHORT).show();
            } else {
                Toast.makeText(this, "Разрешение на интернет не предоставлено", Toast.LENGTH_LONG).show();
            }
        }
    }


    private class SendEmailTask extends AsyncTask<String, Void, String> {

        @Override
        protected void onPreExecute() {
            Toast.makeText(MainActivity.this, "Отправка письма...", Toast.LENGTH_SHORT).show();
        }

        @Override
        protected String doInBackground(String... params) {
            String recipient = params[0];
            String subject = params[1];
            String message = params[2];


            Properties props = new Properties();
            props.put("mail.smtp.auth", "true");
            props.put("mail.smtp.starttls.enable", "true");
            props.put("mail.smtp.host", "smtp.mail.ru");
            props.put("mail.smtp.port", "587");


            Session session = Session.getInstance(props, new jakarta.mail.Authenticator() {
                @Override
                protected PasswordAuthentication getPasswordAuthentication() {
                    return new PasswordAuthentication(SENDER_EMAIL, SENDER_PASSWORD);
                }
            });

            try {

                Message mimeMessage = new MimeMessage(session);
                mimeMessage.setFrom(new InternetAddress(SENDER_EMAIL));
                mimeMessage.setRecipients(Message.RecipientType.TO, InternetAddress.parse(recipient));
                mimeMessage.setSubject(subject);
                mimeMessage.setText(message);


                Transport.send(mimeMessage);
                return "Письмо успешно отправлено!";
            } catch (MessagingException e) {
                Log.e(TAG, "Ошибка отправки письма", e);
                return "Ошибка: " + e.getMessage() + "\nСтек: " + Log.getStackTraceString(e);
            } catch (Exception e) {
                Log.e(TAG, "Неожиданная ошибка", e);
                return "Неожиданная ошибка: " + e.getMessage() + "\nСтек: " + Log.getStackTraceString(e);
            }
        }

        @Override
        protected void onPostExecute(String result) {
            Toast.makeText(MainActivity.this, result, Toast.LENGTH_LONG).show();
        }
    }
}