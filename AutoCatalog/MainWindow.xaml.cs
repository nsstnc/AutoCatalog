﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using Newtonsoft.Json;
using System.Collections;

namespace AutoCatalog
{

    
    public partial class MainWindow : Window
    {
        // флаг открытого каталога
        bool openedCatalog = false;

        
        DataService dbdata = new DataService(new CatalogAppContext());
        // переменная для текущей подвески
        SuspensionAndBrake? currentSuspensionAndBrakes;
        Manufacturer? currentManufacturer;
        // переменная для текущей комплектации
        Configuration ? currentConfiguration;
        
        // переменная для текущего двигателя
        Engine? currentEngine;
        // переменная для текущей коробки передач
        Transmission? currentTransmission;

        // объявляем делегат удаления
        delegate void deleteFrom();
        deleteFrom deleteFunction;

        // объявляем делегат изменения
        delegate void changeFrom(object sender, RoutedEventArgs e);
        changeFrom changeFunction;

        
        public MainWindow()
        {
            InitializeComponent();
            


            fillComboBoxes();

            // задаем ресурсы каталога и производителей для отображения в ListBox
            catalogList.Items.Add(dbdata.GetAllCars());
            manufactureList.Items.Add(dbdata.GetAllManufactures());

            // обновляем производителей
            updateManufactures();

            // обновляем каталог
            updateCatalog();
        }
        

        private void fillComboBoxes()
        {
            // наполняем комбобоксы
            bodiesComboBox.ItemsSource = (new List<string> { 
                "Седан", 
                "Хэтчбек 5 дв." , 
                "Хэтчбек 3 дв.",
                "Лифтбек",
                "Внедорожник 3 дв.",
                "Внедорожник 5 дв.",
                "Универсал",
                "Купе",
                "Минивэн",
                "Пикап",
                "Лимузин",
                "Фургон",
                "Кабриолет"});

            typeOfDriveConfigComboBox.ItemsSource = (new List<string> {
               "Полный",
                "Передний",
                "Задний",
            });

            typeOfDriveConfigComboBox.ItemsSource = (new List<string> {
               "Полный",
                "Передний",
                "Задний",
            });

            typeEngineTextBox.ItemsSource = (new List<string> {
                "ДВС",
                "Гибрид",
                "Электродвигатель",
            });

            cylindersEngineTextBox.ItemsSource = (new List<string>
            {
                 "Рядный",
                "Оппозитный",
                "V-образный",
                "VR-образный",
                "W-образный",
                "Роторный",
            });

            typeOfBoostEngineTextBox.ItemsSource = (new List<string>
            {
                  "Резонансный",
                "Механический",
                "Газотурбинный",
            });

            enginePowerSupplySystemEngineTextBox.ItemsSource = (new List<string>
            {
                 "Бензин",
                "Дизель",
                "Газ",
                "Электричество",
            });

            typeTransmissionTextBox.ItemsSource = (new List<string>
            {
                "Автоматическая",
                "Механическая",
                "Роботизированная",
                "Вариативная",
            });

            typeOfFrontSuspensionComboBox.ItemsSource = (new List<string>
            {
                "Зависимая",
                "Полунезависимая",
                "Независимая",
            });

            typeOfBackSuspensionComboBox.ItemsSource = (new List<string>
            {
               "Зависимая",
                "Полунезависимая",
                "Независимая",
            });

            frontBrakesComboBox.ItemsSource = (new List<string>
            {
               "Дисковые",
                "Барабанные",
            });

            backBrakesComboBox.ItemsSource = (new List<string>
            {
               "Дисковые",
                "Барабанные",
            });
        }



        private void dropSelector()
        {
            catalogList.SelectedIndex = -1;
            manufactureList.SelectedIndex = -1;
        }

        // обработчик нажатия по окну приложения, мимо кнопок
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            dropSelector();
        }


        // функция для закрытия всех страниц
        private void hideAllPages()
        {
            manufactureList.Visibility = Visibility.Hidden;
            catalogList.Visibility = Visibility.Hidden;
            catalogAddingPanel.Visibility = Visibility.Hidden;
            manufacturesAddingPanel.Visibility = Visibility.Hidden;
            configurationCreatingPanel.Visibility = Visibility.Hidden;
            engineCreatingPanel.Visibility = Visibility.Hidden;
            transmissionCreatingPanel.Visibility = Visibility.Hidden;
            suspensionAndBrakesCreatingPanel.Visibility = Visibility.Hidden;
        }



        // метод создания краткой информации из свойств объекта
        private string createInfo(object obj)
        {
            string info = "";
            
            foreach (var property in obj.GetType().GetProperties())
            {
                var value = property.GetValue(obj, null);
                // если значение свойства строка или число, добавляем его в инфо
                if ((value is string) || (value is int) || (value is double)) info += value + ", ";
                
            }

           
            return info;
        }


        // метод очищения полей в окне
        private void clearChildrenBoxes(StackPanel panel) 
        { 
            foreach(var child in panel.Children) 
            {
                if (child is TextBox) ((TextBox)child).Clear();
                if (child is ComboBox) ((ComboBox)child).SelectedValue = "";
            }
        }







                                                    /*  ОКНО ДОБАВЛЕНИЯ ПРОИЗВОДИТЕЛЕЙ   */



        // метод обновления списка производителей
        private void updateManufactures()
        {
            manufacturesComboBox.Items.Clear();
            manufactureList.Items.Clear();
            // конфиг кнопки внутри ComboBox
            Button x = new Button();
            x.Content = "Добавить";
            x.Click += addInManufacturers;

            manufacturesComboBox.Items.Add(x);

            var manufactures = dbdata.GetAllManufactures();
            foreach (var manufacturer in manufactures)
            {
                manufacturesComboBox.Items.Add(manufacturer.Name);
                manufactureList.Items.Add(manufacturer);
            }
        }

        // кнопка открытия окна добавления производителя в список
        private void addInManufacturers(object sender, RoutedEventArgs e)
        {
            clearChildrenBoxes(manufacturesAddingPanel);
            // закрываем одно окно и открываем другое
            hideAllPages();
            manufacturesAddingPanel.Visibility = Visibility.Visible;
            

        }



        // закрытие окна добавления производителя
        private void cancelButtonManufacture_Click(object sender, RoutedEventArgs e)
        {
            

            // закрываем окно добавления производителя
            hideAllPages();
            // открываем необходимое окно
            if (openedCatalog) { catalogAddingPanel.Visibility = Visibility.Visible; }
            else manufactureList.Visibility = Visibility.Visible;

            // блокируем кнопку изменения
            manufactureChangeButton.IsEnabled = false;
            // блокируем кнопку добавления
            manufactureAddButton.IsEnabled = false;
        }

        // кнопка подтверждения добавления производителя в список производителей
        private void addButtonManufacture_Click(object sender, RoutedEventArgs e)
        {
            
            // создаем экземпляр нового производителя
            Manufacturer manufacturer = new Manufacturer() { Name = nameManufactureTextBox.Text, YearOfFoundation = int.Parse(yearOfFoundationTextBox.Text), Country = countryTextBox.Text };
            dbdata.AddManufacturer(manufacturer);

            updateManufactures();

            // закрываем окно
            cancelButtonManufacture_Click(sender, e);
           

            // в качестве выбранного элемента задаем последний
            manufacturesComboBox.SelectedIndex = dbdata.GetAllManufactures().Count;
            
        }





                                                                                                        /* ОКНО ДОБАВЛЕНИЯ В КАТАЛОГ */

        // метод обновления каталога
        private void updateCatalog() 
        { 
            catalogList.Items.Clear();
            var cars = dbdata.GetAllCars();
            foreach (CarTemplate car in cars) catalogList.Items.Add(car);
        }



        // окно добавления автомобиля в каталог
        private void addInCatalog(object sender, RoutedEventArgs e)
        {
            hideAllPages();
            // очищаем форму
            clearChildrenBoxes(catalogAddingPanel);
            // открываем окно
            catalogAddingPanel.Visibility = Visibility.Visible;

            // сбрасываем комбобоксы и кнопки в них
            if ( configurationComboBox.Items.Count == 2) { configurationComboBox.Items.RemoveAt(1); createConfig.Content = "Создать"; }
            if ( engineComboBox.Items.Count == 2) { engineComboBox.Items.RemoveAt(1); createEngineButton.Content = "Создать"; }
            if ( transmissionComboBox.Items.Count == 2) { transmissionComboBox.Items.RemoveAt(1); createTransmissionButton.Content = "Создать"; }
            if ( suspensionAndBrakesComboBox.Items.Count == 2) { suspensionAndBrakesComboBox.Items.RemoveAt(1); createSuspensionAndBrakesButton.Content = "Создать"; }
            clearChildrenBoxes(engineCreatingPanel);
            clearChildrenBoxes(transmissionCreatingPanel);
            clearChildrenBoxes(suspensionAndBrakesCreatingPanel);
            // обнуляем текущие экземпляры агрегатов
            currentConfiguration = null;
            currentEngine = null;
            currentTransmission = null;
            currentSuspensionAndBrakes = null;
            currentManufacturer = null;

            openedCatalog = true;
        }

        // закрытие окна добавления автомобиля в каталог
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            hideAllPages();
            catalogList.Visibility= Visibility.Visible;



            // блокируем кнопку изменения
            changeButton.IsEnabled = false;
            // блокируем кнопку добавления
            addButton.IsEnabled = false;
  
        }


        // кнопка подтверждения добавления автомобиля в список
        private void addButton_Click(object sender, RoutedEventArgs e)
        {

            addButton.IsEnabled = true;

            Car car = new Car() { Name = nameTextBox.Text, 
                Generation = generationTextBox.Text, 
                ManufacturerId = manufacturesComboBox.SelectedIndex - 1,
                Year = int.Parse(yearTextBox.Text),
                ConfigurationId = currentConfiguration.Id,
                Body = bodiesComboBox.Text,
                Category = categoryTextBox.Text};

            dbdata.AddCar(car);

            updateCatalog();
            hideAllPages();
            catalogList.Visibility = Visibility.Visible;

            clearChildrenBoxes(catalogAddingPanel);
            // сбрасываем комбобоксы и кнопки в них
            configurationComboBox.Items.RemoveAt(1);
            engineComboBox.Items.RemoveAt(1);
            transmissionComboBox.Items.RemoveAt(1);
            suspensionAndBrakesComboBox.Items.RemoveAt(1);
            createConfig.Content = "Создать";
            createEngineButton.Content = "Создать";
            createTransmissionButton.Content = "Создать";
            createSuspensionAndBrakesButton.Content = "Создать";
            
        }

       
                                                                                    /*  ОКНО СОЗДАНИЯ КОМПЛЕКТАЦИИ   */



        // окно создания комплектации
        private void createConfiguration(object sender, RoutedEventArgs e)
        {
            // закрываем окно добавления автомобиля в каталог
            hideAllPages();

         
            clearChildrenBoxes(configurationCreatingPanel);

            // открываем окно создания комплектации
            configurationCreatingPanel.Visibility = Visibility.Visible;


            // забиваем поля значениями текущей комплектации, если она есть

            if (currentConfiguration != null)
            {
                nameConfigTextBox.Text = currentConfiguration.Name;


                // поле двигателя
                string info = createInfo(currentEngine);
                string h = info.Substring(0, Math.Min(40, info.Length)) + "...";
                createEngineButton.Content = "Изменить";
                if (engineComboBox.Items.Count == 1) engineComboBox.Items.Add(h);
                else engineComboBox.Items[engineComboBox.Items.Count - 1] = h;
                // в качестве выбранного элемента задаем последний
                engineComboBox.SelectedIndex = engineComboBox.Items.Count - 1;
                // заполняем поля текущими данными
                if (currentEngine != null)
                {
                    typeEngineTextBox.SelectedIndex = typeEngineTextBox.Items.IndexOf(currentEngine.TypeOfEngine);
                    cylindersEngineTextBox.SelectedIndex = cylindersEngineTextBox.Items.IndexOf(currentEngine.CylinderArrangement);
                    powerEngineTextBox.Text = currentEngine.Power.ToString();
                    volumeEngineTextBox.Text = currentEngine.Volume.ToString();
                    torqueEngineTextBox.Text = currentEngine.MaxTorque.ToString();
                    numberOfCylindersEngineTextBox.Text = currentEngine.NumberOfCylinders.ToString();
                    typeOfBoostEngineTextBox.SelectedIndex = typeOfBoostEngineTextBox.Items.IndexOf(currentEngine.TypeOfBoost);
                    fuelGradeEngineTextBox.Text = currentEngine.FuelGrade.ToString();
                    enginePowerSupplySystemEngineTextBox.SelectedIndex = enginePowerSupplySystemEngineTextBox.Items.IndexOf(currentEngine.EnginePowerSupplySystem);
                }





                // поле трансмиссии
                info = createInfo(currentTransmission);
                h = info.Substring(0, Math.Min(40, info.Length)) + "...";
                createTransmissionButton.Content = "Изменить";
                if (transmissionComboBox.Items.Count == 1) transmissionComboBox.Items.Add(h);
                else transmissionComboBox.Items[transmissionComboBox.Items.Count - 1] = h;
                // в качестве выбранного элемента задаем последний
                transmissionComboBox.SelectedIndex = transmissionComboBox.Items.Count - 1;
                // заполняем поля текущими данными
                if (currentTransmission != null)
                {
                    typeTransmissionTextBox.SelectedIndex = typeTransmissionTextBox.Items.IndexOf(currentTransmission.Type);
                    numberOfGearsTransmissionTextBox.Text = currentTransmission.NumberOfGears.ToString();
                }




                // поле подвески
                info = createInfo(currentSuspensionAndBrakes);
                h = info.Substring(0, Math.Min(40, info.Length)) + "...";
                createSuspensionAndBrakesButton.Content = "Изменить";
                if (suspensionAndBrakesComboBox.Items.Count == 1) suspensionAndBrakesComboBox.Items.Add(h);
                else suspensionAndBrakesComboBox.Items[suspensionAndBrakesComboBox.Items.Count - 1] = h;
                // в качестве выбранного элемента задаем последний
                suspensionAndBrakesComboBox.SelectedIndex = suspensionAndBrakesComboBox.Items.Count - 1;
                // заполняем поля текущими данными
                if (currentSuspensionAndBrakes != null)
                {
                    typeOfFrontSuspensionComboBox.SelectedIndex = typeOfFrontSuspensionComboBox.Items.IndexOf(currentSuspensionAndBrakes.TypeOfFrontSuspension);
                    typeOfBackSuspensionComboBox.SelectedIndex = typeOfBackSuspensionComboBox.Items.IndexOf(currentSuspensionAndBrakes.TypeOfBackSuspension);
                    frontBrakesComboBox.SelectedIndex = frontBrakesComboBox.Items.IndexOf(currentSuspensionAndBrakes.FrontBrakes);
                    backBrakesComboBox.SelectedIndex = backBrakesComboBox.Items.IndexOf(currentSuspensionAndBrakes.BackBrakes);
                }


                typeOfDriveConfigComboBox.SelectedIndex = typeOfDriveConfigComboBox.Items.IndexOf(currentConfiguration.TypeOfDrive);
                overclockingConfigTextBox.Text = currentConfiguration.OverClocking.ToString();
                clearanceConfigTextBox.Text = currentConfiguration.Clearance.ToString();
                curbWeightConfigTextBox.Text = currentConfiguration.CurbWeight.ToString();
                fullWeightConfigTextBox.Text = currentConfiguration.FullWeight.ToString();
                fuelTankVolumeConfigTextBox.Text = currentConfiguration.FuelTankVolume.ToString();
                numberOfSeatsConfigTextBox.Text = currentConfiguration.NumberOfSeats.ToString();
            }
        }

        // отмена создания комплектации
        private void cancelButtonConfig_Click(object sender, RoutedEventArgs e)
        {
            // закрываем окно создания комплектации
            hideAllPages();
            // открываем окно добавления автомобиля в каталог
            catalogAddingPanel.Visibility = Visibility.Visible;
        }

        // подтверждение создания комплектации
        private void createButtonConfig_Click(object sender, RoutedEventArgs e)
        {
            

            currentConfiguration = new Configuration(){
                Id = currentConfiguration?.Id,
                Name = nameConfigTextBox.Text,
                Engine = currentEngine,
                TransmissionId = currentTransmission.Id,
                SuspensionAndBrakesId = currentSuspensionAndBrakes.Id,
                TypeOfDrive = typeOfDriveConfigComboBox.Text,
                OverClocking = decimal.Parse(overclockingConfigTextBox.Text),
                Clearance = int.Parse(clearanceConfigTextBox.Text),
                CurbWeight = int.Parse(curbWeightConfigTextBox.Text),
                FullWeight = int.Parse(fullWeightConfigTextBox.Text),
                FuelTankVolume = int.Parse(fuelTankVolumeConfigTextBox.Text),
                NumberOfSeats = int.Parse(numberOfSeatsConfigTextBox.Text)
                };

            // добавляем запись в базу данных
            dbdata.AddConfiguration(currentConfiguration);

            string info = createInfo(currentConfiguration);
            string h = info.Substring(0, Math.Min(40, info.Length)) + "...";
            // если ни одна комплектация еще не была добавлена (длина комбобокса = 1, в нем только кнопка)
            if (configurationComboBox.Items.Count == 1)
            {
                // тогда меняем содержимое кнопки и добавляем в комбобокс комплектацию
                createConfig.Content = "Изменить";
                configurationComboBox.Items.Add(h);
            }
            else 
            {
                // иначе изменяем последний элемент комбобокса
                configurationComboBox.Items[configurationComboBox.Items.Count - 1] = h;
            };
            
            // в качестве выбранного элемента задаем последний
            configurationComboBox.SelectedIndex = configurationComboBox.Items.Count - 1;
            // закрываем окно
            cancelButtonConfig_Click(sender, e);
            
        }



                                                                                        /*  ОКНО СОЗДАНИЯ ДВИГАТЕЛЯ   */





        // окно создания двигателя
        private void createEngine(object sender, RoutedEventArgs e)
        {
            // закрываем окно создания комплектации
            hideAllPages();
            // открываем окно создания двигателя
            engineCreatingPanel.Visibility = Visibility.Visible;
            
        }

        // отмена создания двигателя
        private void cancelButtonEngine_Click(object sender, RoutedEventArgs e)
        {
            // закрываем окно создания комплектации
            hideAllPages();
            // открываем окно добавления автомобиля в каталог
            configurationCreatingPanel.Visibility = Visibility.Visible;
        }

        // подтверждение создания двигателя
        private void createButtonEngine_Click(object sender, RoutedEventArgs e)
        {

            // задаем новый экземпляр текущего двигателя
            currentEngine = new Engine() {
                TypeOfEngine = typeEngineTextBox.Text.ToString(),
                CylinderArrangement = cylindersEngineTextBox.Text.ToString(),
                Power = int.Parse(powerEngineTextBox.Text),
                Volume = int.Parse(volumeEngineTextBox.Text),
                MaxTorque = int.Parse(torqueEngineTextBox.Text),
                NumberOfCylinders = int.Parse(numberOfCylindersEngineTextBox.Text),
                TypeOfBoost = typeOfBoostEngineTextBox.Text.ToString(),
                FuelGrade = fuelGradeEngineTextBox.Text.ToString(),
                EnginePowerSupplySystem = enginePowerSupplySystemEngineTextBox.Text.ToString()};

            // добавляем запись в базу данных
            dbdata.AddEngine(currentEngine);

            string info = createInfo(currentEngine);
            string h = info.Substring(0, Math.Min(40, info.Length)) + "...";

            // если ни один двигатель еще не был добавлен (длина комбобокса = 1, в нем только кнопка)
            if (engineComboBox.Items.Count == 1)
            {
                // тогда меняем содержимое кнопки и добавляем в комбобокс комплектацию
                createEngineButton.Content = "Изменить";
                engineComboBox.Items.Add(h);
            }
            else
            {
                // иначе изменяем последний элемент комбобокса
                engineComboBox.Items[engineComboBox.Items.Count - 1] = h;
            };

            // в качестве выбранного элемента задаем последний
            engineComboBox.SelectedIndex = engineComboBox.Items.Count - 1;
            // закрываем окно
            cancelButtonEngine_Click(sender, e);
            
        }




                                                                        /*  ОКНО СОЗДАНИЯ КОРОБКИ ПЕРЕДАЧ   */




        // окно создания коробки передач
        private void createTransmission(object sender, RoutedEventArgs e)
        {
            // закрываем окно создания комплектации

            hideAllPages();
            // открываем окно создания трансмиссии
            transmissionCreatingPanel.Visibility = Visibility.Visible;
   
       

        }

        // отмена создания коробки передач
        private void cancelButtonTransmission_Click(object sender, RoutedEventArgs e)
        {
            // закрываем окно создания комплектации
            hideAllPages();
            // открываем окно добавления автомобиля в каталог
            configurationCreatingPanel.Visibility = Visibility.Visible;
        }

        // подтверждение создания коробки передач
        private void createButtonTransmission_Click(object sender, RoutedEventArgs e)
        {
            
            // задаем новый экземпляр текущей коробки передач
            currentTransmission = new Transmission() {
                Type = typeTransmissionTextBox.Text,
                NumberOfGears = int.Parse(numberOfGearsTransmissionTextBox.Text)
            };
            // добавляем запись в базу данных
            dbdata.AddTransmission(currentTransmission);

            // если ни одна коробка еще не был добавлена (длина комбобокса = 1, в нем только кнопка)
            if (transmissionComboBox.Items.Count == 1)
            {
                // тогда меняем содержимое кнопки и добавляем в комбобокс комплектацию
                createTransmissionButton.Content = "Изменить";
                transmissionComboBox.Items.Add(createInfo(currentTransmission));
            }
            else
            {
                // иначе изменяем последний элемент комбобокса
                transmissionComboBox.Items[transmissionComboBox.Items.Count - 1] = createInfo(currentTransmission);
            };

            // в качестве выбранного элемента задаем последний
            transmissionComboBox.SelectedIndex = transmissionComboBox.Items.Count - 1;
            // закрываем окно
            cancelButtonTransmission_Click(sender, e);
            
        }


                                                                                    /*  ОКНО СОЗДАНИЯ ПОДВЕСКИ И ТОРМОЗОВ   */
        



        // окно создания подвески и тормозов
        private void createSuspensionAndBrakes(object sender, RoutedEventArgs e)
        {
            // закрываем окно создания комплектации
            hideAllPages();
            // открываем окно создания подвески и тормозов
            suspensionAndBrakesCreatingPanel.Visibility = Visibility.Visible;

        }

        // отмена создания подвески и тормозов
        private void cancelButtonSuspensionAndBrakes_Click(object sender, RoutedEventArgs e)
        {
            // закрываем окно создания комплектации
            hideAllPages();
            // открываем окно добавления автомобиля в каталог
            configurationCreatingPanel.Visibility = Visibility.Visible;
        }

        // подтверждение создания подвески и тормозов
        private void createButtonSuspensionAndBrakes_Click(object sender, RoutedEventArgs e)
        {
            // задаем новый экземпляр текущей коробки передач
            currentSuspensionAndBrakes = new SuspensionAndBrake(){
                //Id = dbdata.SuspensionsCount(),
                TypeOfFrontSuspension = typeOfFrontSuspensionComboBox.Text.ToString(),
                TypeOfBackSuspension = typeOfBackSuspensionComboBox.Text.ToString(),
                BackBrakes = backBrakesComboBox.Text.ToString(),
                FrontBrakes = frontBrakesComboBox.Text.ToString()
            };
            // добавляем запись в базу данных
            dbdata.AddSuspensionAndBrakes(currentSuspensionAndBrakes);

            string info = createInfo(currentSuspensionAndBrakes);
            string h = info.Substring(0, Math.Min(40, info.Length)) + "...";
            // если ни один двигатель еще не был добавлен (длина комбобокса = 1, в нем только кнопка)
            if (suspensionAndBrakesComboBox.Items.Count == 1)
            {
                // тогда меняем содержимое кнопки и добавляем в комбобокс подвеску
                createSuspensionAndBrakesButton.Content = "Изменить";
                suspensionAndBrakesComboBox.Items.Add(h);
            }
            else
            {
                // иначе изменяем последний элемент комбобокса
                suspensionAndBrakesComboBox.Items[suspensionAndBrakesComboBox.Items.Count - 1] = h;
            };

            // в качестве выбранного элемента задаем последний
            suspensionAndBrakesComboBox.SelectedIndex = suspensionAndBrakesComboBox.Items.Count - 1;
            // закрываем окно
            cancelButtonSuspensionAndBrakes_Click(sender, e);
        }




        /* ЗАКРЫТИЕ ОКНА */

        
        // действие при закрытии приложения
        private void Window_Close(object sender, CancelEventArgs e)
        {
          
        }
        



        /* ОКНО ОТОБРАЖЕНИЯ КАТАЛОГА*/

        private void showCatalog(object sender, RoutedEventArgs e)
        {
            dropSelector();
            if (catalogList.IsVisible)
            { 
                hideAllPages();
            }
            else
            {
                hideAllPages();
                catalogList.Visibility = Visibility.Visible;
                AddButton.Visibility = Visibility.Visible;
                
            }
            
        }
        // удаление из каталога
        private void deleteFromCatalog()
        {
            
            // сохраняем индекс текущего выбранного элемента
            int selected = catalogList.SelectedIndex;

            CarTemplate current = (CarTemplate)catalogList.Items[selected];

            currentConfiguration = dbdata.GetConfigByCar(current);
            currentManufacturer = dbdata.GetManufacturerByCar(current);
            currentEngine = dbdata.GetEngineByConfig(currentConfiguration);
            currentSuspensionAndBrakes = dbdata.GetSuspensionByConfig(currentConfiguration);
            currentTransmission = dbdata.GetTransmissionByConfig(currentConfiguration);




            // удаляем элемент
            dbdata.DeleteCar(current);




            // обновляем список
            updateCatalog();
            updateManufactures();



            // задаем новый выбранный элемент предыдущему
            catalogList.SelectedIndex = selected - 1;
            catalogList.Focus();
            
        }

        // изменение из каталога
        private void changeFromCatalog(object sender, RoutedEventArgs e)
        {
           
            // делаем активной кнопку изменения
            changeButton.IsEnabled = true;

            // сохраняем индекс текущего выбранного элемента
            int selected = catalogList.SelectedIndex;
            // открываем окно формы каталога
            addInCatalog(sender, e);

            // текущий объект автомобиля и текущие экземпляры классов нижних уровней
            CarTemplate current = (CarTemplate)catalogList.Items[selected];



            currentConfiguration = dbdata.GetConfigByCar(current);
            currentManufacturer = dbdata.GetManufacturerByCar(current);

            log.Text = currentManufacturer.Name;

            currentEngine = dbdata.GetEngineByConfig(currentConfiguration);
            currentSuspensionAndBrakes = dbdata.GetSuspensionByConfig(currentConfiguration);
            currentTransmission = dbdata.GetTransmissionByConfig(currentConfiguration);

            // заполняем поля текущими данными
            nameTextBox.Text = current.Name;
            generationTextBox.Text = current.Generation;
            yearTextBox.Text = current.Year.ToString();
            manufacturesComboBox.SelectedIndex = manufacturesComboBox.Items.IndexOf(currentManufacturer.Name);



            // заполнение поля конфигурации
            string info = createInfo(currentConfiguration);
            string h = info.Substring(0, Math.Min(40, info.Length)) + "..."; createConfig.Content = "Изменить";
            if (configurationComboBox.Items.Count == 1) configurationComboBox.Items.Add(h);
            else configurationComboBox.Items[configurationComboBox.Items.Count - 1] = h;
            // в качестве выбранного элемента задаем последний
            configurationComboBox.SelectedIndex = configurationComboBox.Items.Count - 1;


            bodiesComboBox.SelectedIndex = bodiesComboBox.Items.IndexOf(current.Body);
            categoryTextBox.Text = current.Category;

           
        }
        // подтверждение изменения
        private void changeButton_Click(object sender, RoutedEventArgs e)
        {
            
            // сохраняем индекс текущего выбранного элемента
            int selected = catalogList.SelectedIndex;

            

            // создаем новый экземпляр класса с новыми заполненными данными
            Car car = new Car()
            { 
                Name = nameTextBox.Text,
                Generation = generationTextBox.Text,
                ManufacturerId = dbdata.GetManufacturerById((ManufacturerTemplate)manufactureList.Items[manufacturesComboBox.SelectedIndex - 1]),
                Year = int.Parse(yearTextBox.Text),
                ConfigurationId = currentConfiguration.Id,
                Body = bodiesComboBox.Text,
                Category = categoryTextBox.Text
             };

            





            // заменяем элемент
            dbdata.UpdateCar(car, currentEngine, currentTransmission, currentSuspensionAndBrakes, currentConfiguration);
            // обновляем каталог
            updateCatalog();

           
            // закрываем окно
            cancelButton_Click(sender, e);
            // ставим фокус списка на текущий элемент
            catalogList.SelectedIndex = selected;
            catalogList.Focus();
            
        }


        // обработчик изменения выбранного элемента
        private void catalog_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            activityOfButtons();
        }
  

                                        /* ОКНО ОТОБРАЖЕНИЯ ПРОИЗВОДИТЕЛЕЙ */




        private void showManufactures(object sender, RoutedEventArgs e)
        {
            dropSelector();
            if (manufactureList.IsVisible)
            {
                hideAllPages();
            }
            else
            {
                hideAllPages();
                openedCatalog = false;
                manufactureList.Visibility = Visibility.Visible;
                AddButton.Visibility = Visibility.Visible;
                DeleteButton.Visibility = Visibility.Visible;
            }
        }
        // удаление из производителей
        private void deleteFromManufactures()
        {
            
            // сохраняем индекс текущего выбранного элемента
            int selected = manufactureList.SelectedIndex;

            ManufacturerTemplate item = (ManufacturerTemplate)manufactureList.Items[selected];

            dbdata.DeleteManifacturer(item);

            // обновляем список производителей
            updateManufactures();
            // обновляем список каталога
            updateCatalog();

            // задаем новый выбранный элемент предыдущему
            manufactureList.SelectedIndex = selected - 1;
            manufactureList.Focus();
            
        }


        // изменение из производителей
        private void changeFromManufactures(object sender, RoutedEventArgs e)
        {
            
            // делаем активной кнопку изменения
            manufactureChangeButton.IsEnabled = true;

            // сохраняем индекс текущего выбранного элемента
            int selected = manufactureList.SelectedIndex;
            // открываем окно формы производителей
            addInManufacturers(sender, e);
            

            // текущий объект класса производителей
            ManufacturerTemplate current = (ManufacturerTemplate)manufactureList.Items[selected];
            // заполняем поля текущими данными
            nameManufactureTextBox.Text = current.Name;
            yearOfFoundationTextBox.Text = current.YearOfFoundation.ToString();
            countryTextBox.Text = current.Country;
            
        }

        // подтверждение изменения
        private void changeButtonManufacture_Click(object sender, RoutedEventArgs e)
        {

            // сохраняем индекс текущего выбранного элемента
            int selected = manufactureList.SelectedIndex;

            ManufacturerTemplate manufacturer_old = (ManufacturerTemplate)manufactureList.Items[selected];

            // создаем экземпляр производителя с новыми данными из полей
            ManufacturerTemplate manufacturer = new ManufacturerTemplate()
            {
                Name = nameManufactureTextBox.Text, 
                YearOfFoundation = int.Parse(yearOfFoundationTextBox.Text),
                Country = countryTextBox.Text
             };


            // заменяем элемент
            dbdata.UpdateManufacturer(manufacturer_old, manufacturer);

            // обновляем список производителей
            updateManufactures();
            // обновляем каталог
            updateCatalog();
            // закрываем окно
            cancelButtonManufacture_Click(sender, e);
            // ставим фокус на текущий элемент
            manufactureList.SelectedIndex = selected;
            manufactureList.Focus();
            
        }






        // обработчик изменения выбранного элемента
        private void manufacrures_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            activityOfButtons();
        }










        // переключить отображение кнопок удаления и изменения
        private void activityOfButtons()
        {
            // если хоть на одной странице есть выбранный элемент, то показываем кнопки
            if (manufactureList.SelectedIndex != -1 || catalogList.SelectedIndex != -1)
            {
                DeleteButton.IsEnabled = true;
                ChangeButton.IsEnabled = true;
            }
            else
            {
                DeleteButton.IsEnabled = false;
                ChangeButton.IsEnabled = false;
            }
        }

        // кнопка удалить
        private void delete_click(object sender, RoutedEventArgs e)
        {

            if (manufactureList.IsVisible) deleteFunction = deleteFromManufactures;
            else deleteFunction = deleteFromCatalog;
            // запуск делегата
            deleteFunction(); 
        }

        // кнопка добавить
        private void add_click(object sender, RoutedEventArgs e)
        {
            dropSelector();
            if (manufactureList.IsVisible)
            {
                manufactureAddButton.IsEnabled = true;
                addInManufacturers(sender, e);
            }
            else
            {
                addInCatalog(sender, e);
                // делаем кнопку активной
                addButton.IsEnabled = true;
            }
           }

            // кнопка изменить
            private void change_click(object sender, RoutedEventArgs e)
        {
            if (manufactureList.IsVisible) changeFunction = changeFromManufactures;
            else changeFunction = changeFromCatalog;
            // запуск делегата
            changeFunction(sender, e);
        }

        
    }
}
