# Upscale Technical Test

Technical test for Upscale (Aplikasi Manajemen Tugas Sederhana)

## Cara Menjalankan Aplikasi

Prasyarat
1. Cek versi .Net dengan menggunakan Command Prompt  
Ketik dan tekan enter: 

```bash
dotnet --version
```

![Screenshot 2023-03-03 234950](https://user-images.githubusercontent.com/10527362/222869310-6834be9c-3389-43c1-899f-557e480e7eba.png)

Notes:
- Pastikan versi .Net sama atau menggunakan versi terbaru.
- Silahkan download di sini jika belum terinstal [dotnet](https://dotnet.microsoft.com/en-us/download/dotnet/7.0).

2. Pastikan Microsoft Sql Server sudah terinstal  
Silahkan download di sini jika belum terinstal [Sql Server](https://go.microsoft.com/fwlink/p/?linkid=2216019&clcid=0x409&culture=en-us&country=us).


Setelah Prasyarat terpenuhi, silahkan Clone Repositori ini dengan Visual Studio atau Visual Studio Code ataupun IDE lain yang mendukung.  
Setelah proses Clone selesai, Silahkan lanjut ke step berikutnya untuk menjalankan aplikasi:  

Notes:  
- Anda dapat langsung menuju ke folder Publish tanpa membuka Visual Studio atau IDE lain untuk menjalankan aplikasi ini.  
- Jika anda membukanya di Visual Studio atau IDE lain, anda bisa menjalankan aplikasi ini dengan F5 atau shortcut CTRL+F5, setelah meyesuaikan konfigurasi pada step 1 berikut ini.  

1. Sesuaikan konfigurasi yang ada di file appsettings.json 

![Screenshot 2023-03-03 234500](https://user-images.githubusercontent.com/10527362/222873098-3c83fa96-14c3-4de0-b24c-46b38c569ecb.png)

Yang perlu di sesuaikan:  
- ConnectionStrings -> DefaultConnection  
- SenderEmailProvider (Sendgrid atau GoogleSMTP)  
- SendgridAPIKey (jika value SenderEmailProvider = Sendgrid)  
- SendgridEmailFrom (jika value SenderEmailProvider = Sendgrid)  
- SendgridEmailName (jika value SenderEmailProvider = Sendgrid)  
- GoogleSMTPEmailAddress (jika value SenderEmailProvider = GoogleSMTP)  
- GoogleSMTPEmailPassword (jika value SenderEmailProvider = GoogleSMTP) 


2. Jalankan Command Prompt di folder Publish  

![Screenshot 2023-03-03 2338139](https://user-images.githubusercontent.com/10527362/222871360-8fc7290d-3fe5-40ff-9218-142075646f25.png)

3. Jalankan aplikasi dengan ketik dan tekan enter di Command Prompt: 

```bash
dotnet .\UpscaleTechnicalTest.dll
```
![Screenshot 2023-03-03 233541](https://user-images.githubusercontent.com/10527362/222871589-8441a006-9a6c-4ce0-a145-0a4e0306fab2.png)

Notes:  
Aplikasi akan melakukan Migrasi database pada saat pertama kali di jalankan.  

4. Silahkan cek database untuk memastikan Migrasi database berjalan lancar

![Screenshot 2023-03-03 234656](https://user-images.githubusercontent.com/10527362/222871894-0013f4ed-5bfe-4625-8e1e-1afb80520a25.png)

5. Cek Command prompt pada step 3 untuk mendapatkan URL aplikasi

![Screenshot 2023-03-03 233813](https://user-images.githubusercontent.com/10527362/222872014-629321ae-560c-4a52-aa44-bdc2665ca3da.png)  

6. Buka browser dan ketikkan URL yang di dapat pada step sebelumnya, atau tekan CTRL+Klik URL pada command prompt

![Screenshot 2023-03-03 233941](https://user-images.githubusercontent.com/10527362/222872166-37bad305-23ec-4c5e-9ba3-7a1f020d7535.png)

7. Silahkan klik menu Todo yang berada di sudut kiri setelah menu Home  
Note:  
-Jika anda belum melakukan Login sebelumnya maka sistem akan menampikan halaman Login, silahkan Login atau Klik opsi Register jika belum memiliki akun.

 ![Screenshot 2023-03-03 234033](https://user-images.githubusercontent.com/10527362/222872358-e1431897-5534-4d2e-8cee-9336625b1c66.png)
 
8. Setelah berhasil Login anda bisa membuat daftar kerjaan (Todo) atau memanage kerjaan yang sudah ada
  
 ![Screenshot 2023-03-03 234055](https://user-images.githubusercontent.com/10527362/222872463-d84b4151-c7d1-460a-b092-18fba361b9c1.png)

9. Sistem ini dilengkapi dengan Sistem Notifikasi menggunakan BackroundService yang berfungsi untuk mengecek tugas yang status IsCompleted-nya "Not Set" atau "False"
dan Deadline-nya sisa 1 Hari atau kurang.

![Screenshot 2023-03-03 172828](https://user-images.githubusercontent.com/10527362/222872731-c975f2fe-2300-401a-b9f1-1d035f96efe5.png)

Berikut sample email yang berhasil terkirim:

![Screenshot 2023-03-03 172847](https://user-images.githubusercontent.com/10527362/222872803-0333a3e7-df78-4266-bedc-472b059efa23.png)

Note:
- Konfigurasi email ada pada step 1
