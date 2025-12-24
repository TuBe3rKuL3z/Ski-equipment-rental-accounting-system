package com.example.android_ski_equipment_rental_accounting_system;

import android.icu.text.SimpleDateFormat;
import android.os.Bundle;
import android.os.Handler;
import android.view.View;
import android.widget.TextView;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.cardview.widget.CardView;
import androidx.constraintlayout.widget.ConstraintLayout;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

import java.util.Date;
import java.util.Locale;

public class MainActivity extends AppCompatActivity {

    private CardView btnMenu;
    private CardView sideMenu;
    private ConstraintLayout mainContent;
    private TextView txtCurrentTime;
    private Handler timeHandler;
    private boolean isMenuOpen = false;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_main);

        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.mainContainer), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });

        initializeViews();
        setupClickListeners();
        startTimeUpdates();
    }

    private void initializeViews() {
        btnMenu = findViewById(R.id.btnMenu);
        sideMenu = findViewById(R.id.sideMenu);
        mainContent = findViewById(R.id.mainContent);
        txtCurrentTime = findViewById(R.id.txtCurrentTime);
    }

    private void setupClickListeners() {
        // Кнопка меню (три полосы)
        btnMenu.setOnClickListener(v -> toggleSideMenu());

        // Кнопка настроек
        CardView btnSettings = findViewById(R.id.btnSettings);
        btnSettings.setOnClickListener(v -> {
            // TODO: Реализовать открытие настроек
            showToast("Открыть настройки");
        });

        // Кнопка удаления
        CardView btnDelete = findViewById(R.id.btnDeleteRow);
        btnDelete.setOnClickListener(v -> {
            // TODO: Реализовать удаление
            showToast("Удалить выбранное");
        });

        // Кнопки навигации в боковом меню
        findViewById(R.id.btnRentals).setOnClickListener(v -> navigateToSection("Аренды"));
        findViewById(R.id.btnBookings).setOnClickListener(v -> navigateToSection("Бронирования"));
        findViewById(R.id.btnClients).setOnClickListener(v -> navigateToSection("Клиенты"));
        findViewById(R.id.btnMaintenance).setOnClickListener(v -> navigateToSection("Обслуживание"));
        findViewById(R.id.btnEquipment).setOnClickListener(v -> navigateToSection("Оборудование"));

        // Кнопки быстрых действий
        CardView btnNewRental = findViewById(R.id.btnQuickNewRental);
        btnNewRental.setOnClickListener(v -> showToast("Создать новую аренду"));

        CardView btnReturn = findViewById(R.id.btnQuickReturn);
        btnReturn.setOnClickListener(v -> showToast("Зарегистрировать возврат"));
    }

    private void toggleSideMenu() {
        if (isMenuOpen) {
            // Закрыть меню
            sideMenu.setVisibility(View.GONE);
            ConstraintLayout.LayoutParams params = (ConstraintLayout.LayoutParams) mainContent.getLayoutParams();
            params.startToStart = ConstraintLayout.LayoutParams.PARENT_ID;
            params.endToEnd = ConstraintLayout.LayoutParams.PARENT_ID;
            mainContent.setLayoutParams(params);
        } else {
            // Открыть меню
            sideMenu.setVisibility(View.VISIBLE);
            ConstraintLayout.LayoutParams params = (ConstraintLayout.LayoutParams) mainContent.getLayoutParams();
            params.startToEnd = R.id.sideMenu;
            params.endToEnd = ConstraintLayout.LayoutParams.PARENT_ID;
            mainContent.setLayoutParams(params);
        }

        // Анимация
        sideMenu.animate()
                .translationX(0)
                .setDuration(300)
                .start();

        isMenuOpen = !isMenuOpen;
    }
    private void navigateToSection(String sectionName) {
        showToast("Переход в раздел: " + sectionName);
        // TODO: Реализовать навигацию по разделам
        // Можно добавить закрытие меню после выбора
        if (isMenuOpen) {
            toggleSideMenu();
        }
    }

    private void startTimeUpdates() {
        timeHandler = new Handler();
        Runnable timeUpdater = new Runnable() {
            @Override
            public void run() {
                updateCurrentTime();
                timeHandler.postDelayed(this, 1000); // Обновлять каждую секунду
            }
        };
        timeHandler.post(timeUpdater);
    }

    private void updateCurrentTime() {
        SimpleDateFormat sdf = new SimpleDateFormat("HH:mm:ss", Locale.getDefault());
        String currentTime = sdf.format(new Date());
        txtCurrentTime.setText(currentTime);
    }

    private void showToast(String message) {
        // Используйте Toast или Snackbar для отображения сообщений
        android.widget.Toast.makeText(this, message, android.widget.Toast.LENGTH_SHORT).show();
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        if (timeHandler != null) {
            timeHandler.removeCallbacksAndMessages(null);
        }
    }
}