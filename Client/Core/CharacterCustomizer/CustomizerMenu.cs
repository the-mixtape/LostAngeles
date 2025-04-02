using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using LostAngeles.Client.Core.Data;
using LostAngeles.Shared;

namespace LostAngeles.Client.Core.CharacterCustomizer
{
    public partial class CharacterCustomizer
    {
        private NativeUI.MenuPool _menuPool;
        private NativeUI.UIMenu _mainMenu;

        private void CreateMainMenu()
        {
            _menuPool = new NativeUI.MenuPool();
            _mainMenu = new NativeUI.UIMenu("Character Creator", "~HUD_COLOR_FREEMODE~EDIT CHARACTER");

            _menuPool.Add(_mainMenu);

            AddGenderMenu();
            AddHeritageMenu();
            AddFaceShapeMenu();
            AddAppearanceMenu();
            AddCompleteButton();
            _menuPool.RefreshIndex();

            _mainMenu.Visible = true;
            _mainMenu.ResetKey(NativeUI.UIMenu.MenuControls.Back);

            Tick += MenuPoolTick;
        }

        private void DeleteMainMenu()
        {
            _mainMenu.Visible = false;

            Tick -= MenuPoolTick;
            
            _menuPool = null;
            _mainMenu = null;
        }

        private async Task MenuPoolTick()
        {
            if (_menuPool == null) return;

            _menuPool.ProcessMenus();
            await Task.FromResult(0);
        }

        private void AddGenderMenu()
        {
            int genderIndex = 0;
            if (ClientData.Character.Gender == FemaleGender)
            {
                genderIndex = 1;
            }

            var genderItem =
                new NativeUI.UIMenuListItem("Sex", _genders, genderIndex, "Select the gender of your Character.");
            _mainMenu.AddItem(genderItem);

            genderItem.OnListChanged += (sender, index) =>
            {
                var selectedGender = _genders[index];
                _ = SetGender(selectedGender);
            };
        }

        private void AddCompleteButton()
        {
            var completeButton = new NativeUI.UIMenuItem("Complete", "Finish and Save.")
            {
                Enabled = true
            };
            _mainMenu.AddItem(completeButton);

            _mainMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == completeButton)
                {
                    TriggerEvent(ClientEvents.CharacterCustomization.EndCustomizeEvent);
                }
            };
        }

        private void AddHeritageMenu()
        {
            var momIndex = 0;
            var dadIndex = 0;

            var heritageMenu = _menuPool.AddSubMenu(_mainMenu, "Heritage", "Select to choose your parents.");
            var heritageWindow = new NativeUI.UIMenuHeritageWindow(momIndex, dadIndex);
            heritageMenu.AddWindow(heritageWindow);

            var momFaces = Parents.Moms.Select(mom => mom.Name).Cast<dynamic>().ToList();
            var dadFaces = Parents.Dads.Select(dad => dad.Name).Cast<dynamic>().ToList();

            var momList = new NativeUI.UIMenuListItem("Mom", momFaces, momIndex, "Select your Mom.");
            var dadList = new NativeUI.UIMenuListItem("Dad", dadFaces, dadIndex, "Select your Dad.");

            var resemblanceItem = new NativeUI.UIMenuSliderHeritageItem("Resemblance",
                "Select if your features are influenced more by your Mother or Father.", true)
            {
                Value = (int)(ClientData.Character.Resemblance * 100)
            };
            var skintoneItem = new NativeUI.UIMenuSliderHeritageItem("Skin Tone",
                "Select if your skin tone is influenced more by your Mother or Father.", true)
            {
                Value = (int)(ClientData.Character.Skintone * 100)
            };

            heritageMenu.AddItem(momList);
            heritageMenu.AddItem(dadList);
            heritageMenu.AddItem(resemblanceItem);
            heritageMenu.AddItem(skintoneItem);

            heritageMenu.OnListChange += (sender, listItem, newIndex) =>
            {
                if (listItem == momList)
                {
                    momIndex = newIndex;
                    var mom = Parents.Moms[momIndex];
                    ClientData.Character.Mom = mom.Index;
                    heritageWindow.Index(momIndex, dadIndex);
                    RefreshPedHead();
                }
                else if (listItem == dadList)
                {
                    dadIndex = newIndex;
                    var dad = Parents.Dads[dadIndex];
                    ClientData.Character.Dad = dad.Index;
                    heritageWindow.Index(momIndex, dadIndex);
                    RefreshPedHead();
                }
            };

            heritageMenu.OnSliderChange += (sender, item, newIndex) =>
            {
                if (item == resemblanceItem)
                {
                    ClientData.Character.Resemblance = resemblanceItem.Value / 100.0f;
                    RefreshPedHead();
                }
                else if (item == skintoneItem)
                {
                    ClientData.Character.Skintone = skintoneItem.Value / 100.0f;
                    RefreshPedHead();
                }
            };

            heritageMenu.OnMenuStateChanged += (oldMenu, newMenu, state) =>
            {
                if (state == NativeUI.MenuState.ChangeForward)
                {
                    SwitchCamera(_faceCamera, _bodyCamera);
                }
                else if (state == NativeUI.MenuState.ChangeBackward)
                {
                    SwitchCamera(_bodyCamera, _faceCamera);
                }
            };
        }

        private void AddFaceShapeMenu()
        {
            NativeUI.UIMenu subMenu = _menuPool.AddSubMenu(_mainMenu, "Face Shapes", "Select to alter your facial Features.");

            AddFaceShapeSlider(subMenu, "Nose Width", "Make changes to your physical Features.", ClientData.Character.Nose1,
                (_, value) => { ChangeFaceShapeValue(value, out ClientData.Character.Nose1, 0); });
            AddFaceShapeSlider(subMenu, "Nose Peak Height", "Make changes to your physical Features.", ClientData.Character.Nose2,
                (_, value) => { ChangeFaceShapeValue(value, out ClientData.Character.Nose2, 1); });
            AddFaceShapeSlider(subMenu, "Nose Peak Length", "Make changes to your physical Features.", ClientData.Character.Nose3,
                (_, value) => { ChangeFaceShapeValue(value, out ClientData.Character.Nose3, 2); });
            AddFaceShapeSlider(subMenu, "Nose Bone Height", "Make changes to your physical Features.", ClientData.Character.Nose4,
                (_, value) => { ChangeFaceShapeValue(value, out ClientData.Character.Nose4, 3); });
            AddFaceShapeSlider(subMenu, "Nose Peak Lowering", "Make changes to your physical Features.",
                ClientData.Character.Nose5,
                (_, value) => { ChangeFaceShapeValue(value, out ClientData.Character.Nose5, 4); });
            AddFaceShapeSlider(subMenu, "Nose Bone Twist", "Make changes to your physical Features.", ClientData.Character.Nose6,
                (_, value) => { ChangeFaceShapeValue(value, out ClientData.Character.Nose6, 5); });

            AddFaceShapeSlider(subMenu, "Eyebrow Depth", "Make changes to your physical Features.",
                ClientData.Character.Eyebrows5,
                (_, value) => { ChangeFaceShapeValue(value, out ClientData.Character.Eyebrows5, 7); });
            AddFaceShapeSlider(subMenu, "Eyebrow Height", "Make changes to your physical Features.",
                ClientData.Character.Eyebrows6,
                (_, value) => { ChangeFaceShapeValue(value, out ClientData.Character.Eyebrows6, 6); });

            AddFaceShapeSlider(subMenu, "Cheekbones Height", "Make changes to your physical Features.",
                ClientData.Character.Cheeks1,
                (_, value) => { ChangeFaceShapeValue(value, out ClientData.Character.Cheeks1, 8); });
            AddFaceShapeSlider(subMenu, "Cheekbones Width", "Make changes to your physical Features.",
                ClientData.Character.Cheeks2,
                (_, value) => { ChangeFaceShapeValue(value, out ClientData.Character.Cheeks2, 9); });

            AddFaceShapeSlider(subMenu, "Cheeks Width", "Make changes to your physical Features.", ClientData.Character.Cheeks3,
                (_, value) => { ChangeFaceShapeValue(value, out ClientData.Character.Cheeks3, 10); });

            AddFaceShapeSlider(subMenu, "Eye Opening", "Make changes to your physical Features.", ClientData.Character.EyeOpen,
                (_, value) => { ChangeFaceShapeValue(value, out ClientData.Character.EyeOpen, 11); });

            AddFaceShapeSlider(subMenu, "Lips Thickness", "Make changes to your physical Features.",
                ClientData.Character.LipsThick,
                (_, value) => { ChangeFaceShapeValue(value, out ClientData.Character.LipsThick, 12); });

            AddFaceShapeSlider(subMenu, "Jaw Bone Width", "Make changes to your physical Features.", ClientData.Character.Jaw1,
                (_, value) => { ChangeFaceShapeValue(value, out ClientData.Character.Jaw1, 13); });
            AddFaceShapeSlider(subMenu, "Jaw Bone Depth", "Make changes to your physical Features.", ClientData.Character.Jaw2,
                (_, value) => { ChangeFaceShapeValue(value, out ClientData.Character.Jaw2, 14); });

            AddFaceShapeSlider(subMenu, "Chin Height", "Make changes to your physical Features.", ClientData.Character.ChinHeight,
                (_, value) => { ChangeFaceShapeValue(value, out ClientData.Character.ChinHeight, 15); });
            AddFaceShapeSlider(subMenu, "Chin Depth", "Make changes to your physical Features.", ClientData.Character.ChinLength,
                (_, value) => { ChangeFaceShapeValue(value, out ClientData.Character.ChinLength, 16); });
            AddFaceShapeSlider(subMenu, "Chin Width", "Make changes to your physical Features.", ClientData.Character.ChinWidth,
                (_, value) => { ChangeFaceShapeValue(value, out ClientData.Character.ChinWidth, 17); });
            AddFaceShapeSlider(subMenu, "Chin Hole Size", "Make changes to your physical Features.",
                ClientData.Character.ChinHole,
                (_, value) => { ChangeFaceShapeValue(value, out ClientData.Character.ChinHole, 18); });

            AddFaceShapeSlider(subMenu, "Neck Thickness", "Make changes to your physical Features.",
                ClientData.Character.NeckThick,
                (_, value) => { ChangeFaceShapeValue(value, out ClientData.Character.NeckThick, 19); });

            subMenu.OnMenuStateChanged += (oldMenu, newMenu, state) =>
            {
                if (state == NativeUI.MenuState.ChangeForward)
                {
                    SwitchCamera(_faceCamera, _bodyCamera);
                }
                else if (state == NativeUI.MenuState.ChangeBackward)
                {
                    SwitchCamera(_bodyCamera, _faceCamera);
                }
            };
        }

        private void AddAppearanceMenu()
        {
            NativeUI.UIMenu subMenu = _menuPool.AddSubMenu(_mainMenu, "Appearance", "Select to change your Appearance.");

            // === Hair ===
            var hairItem = new NativeUI.UIMenuListItem("Hair", Variants.MaleHairNames, ClientData.Character.Hair,
                "Make changes to your Appearance.");
            var hairColorPanel = new NativeUI.UIMenuColorPanel("Color", NativeUI.UIMenuColorPanel.ColorPanelType.Hair);
            hairItem.OnListChanged += (sender, index) =>
            {
                var colorPanel = sender.Panels.First() as NativeUI.UIMenuColorPanel;
                var hairColor = colorPanel?.CurrentSelection ?? 1;
                ClientData.Character.Hair = index;
                ClientData.Character.HairColor1 = hairColor;
                RefreshPedHair();
            };
            hairItem.AddPanel(hairColorPanel);
            subMenu.AddItem(hairItem);

            // === Eyebrows ===
            var eyebrowItem = new NativeUI.UIMenuListItem("Hair", Variants.EyebrowsNames, ClientData.Character.Eyebrows,
                "Make changes to your Appearance.");
            var browColorPanel = new NativeUI.UIMenuColorPanel("Color", NativeUI.UIMenuColorPanel.ColorPanelType.Hair);
            var browPercentageItem = new NativeUI.UIMenuPercentagePanel("Opacity", "0%", "100%")
            {
                Percentage = ClientData.Character.Eyebrows2
            };
            eyebrowItem.OnListChanged += (sender, index) =>
            {
                var colorPanel = sender.Panels[0] as NativeUI.UIMenuColorPanel;
                var percentagePanel = sender.Panels[1] as NativeUI.UIMenuPercentagePanel;
                var percentage = percentagePanel?.Percentage ?? 1.0f;
                var color = colorPanel?.CurrentSelection ?? 1;
                ClientData.Character.Eyebrows = sender.Index;
                ClientData.Character.Eyebrows2 = percentage;
                ClientData.Character.Eyebrows3 = color;
                RefreshPedEyebrows();
            };
            eyebrowItem.AddPanel(browColorPanel);
            eyebrowItem.AddPanel(browPercentageItem);
            subMenu.AddItem(eyebrowItem);

            // === Beard ===
            var beardItem = new NativeUI.UIMenuListItem("Facial Hair", Variants.BeardNames, ClientData.Character.Beard,
                "Make changes to your Appearance.");
            beardItem.OnListChanged += (sender, index) =>
            {
                if (ClientData.Character.Gender == FemaleGender)
                {
                    ClientData.Character.Beard = 0;
                    beardItem.Index = ClientData.Character.Beard;
                    // UI.ShowNotification("Facial Hair unavailable for Female characters.");
                }
                else
                {
                    ClientData.Character.Beard = sender.Index;
                    if (index == 0)
                    {
                        beardItem.RemovePanelAt(0);
                        beardItem.RemovePanelAt(0);
                        ClientData.Character.Beard2 = 0;
                        ClientData.Character.Beard3 = 0;
                    }
                    else
                    {
                        if (sender.Panels.Count == 0)
                        {
                            var beardColorPanel = new NativeUI.UIMenuColorPanel("Color", NativeUI.UIMenuColorPanel.ColorPanelType.Hair);
                            var beardPercentageItem = new NativeUI.UIMenuPercentagePanel("Opacity", "0%", "100%")
                            {
                                Percentage = ClientData.Character.Eyebrows2
                            };
                            beardItem.AddPanel(beardColorPanel);
                            beardItem.AddPanel(beardPercentageItem);
                        }

                        var colorPanel = sender.Panels[0] as NativeUI.UIMenuColorPanel;
                        var percentagePanel = sender.Panels[1] as NativeUI.UIMenuPercentagePanel;
                        var percentage = percentagePanel?.Percentage ?? 1.0f;
                        var color = colorPanel?.CurrentSelection ?? 1;
                        ClientData.Character.Beard2 = percentage;
                        ClientData.Character.Beard3 = color;
                    }

                    RefreshPedBeard();
                }
            };
            subMenu.AddItem(beardItem);

            // === Blemishes ===
            var blemishesItem = new NativeUI.UIMenuListItem("Skin Blemishes", Variants.Blemishes, ClientData.Character.Bodyb1,
                "Make changes to your Appearance.");
            blemishesItem.OnListChanged += (sender, index) =>
            {
                ClientData.Character.Bodyb1 = index;
                if (index == 0)
                {
                    blemishesItem.RemovePanelAt(0);
                    ClientData.Character.Bodyb2 = 0;
                }
                else
                {
                    if (blemishesItem.Panels.Count == 0)
                    {
                        var percentageItem = new NativeUI.UIMenuPercentagePanel("Opacity", "0%", "100%")
                        {
                            Percentage = ClientData.Character.Bodyb2
                        };
                        blemishesItem.AddPanel(percentageItem);
                    }

                    var percentagePanel = sender.Panels[0] as NativeUI.UIMenuPercentagePanel;
                    var percentage = percentagePanel?.Percentage ?? 1.0f;
                    ClientData.Character.Bodyb2 = percentage;
                }

                RefreshBlemishes();
            };
            subMenu.AddItem(blemishesItem);

            // === Aging ===
            var agingItem = new NativeUI.UIMenuListItem("Skin Aging", Variants.AgingNames, ClientData.Character.Age1,
                "Make changes to your Appearance.");
            agingItem.OnListChanged += (sender, index) =>
            {
                ClientData.Character.Age1 = index;
                if (index == 0)
                {
                    agingItem.RemovePanelAt(0);
                    ClientData.Character.Age2 = 0;
                }
                else
                {
                    if (agingItem.Panels.Count == 0)
                    {
                        var agingPercentageItem = new NativeUI.UIMenuPercentagePanel("Opacity", "0%", "100%")
                        {
                            Percentage = ClientData.Character.Age2
                        };
                        agingItem.AddPanel(agingPercentageItem);
                    }

                    var percentagePanel = sender.Panels[0] as NativeUI.UIMenuPercentagePanel;
                    var percentage = percentagePanel?.Percentage ?? 1.0f;
                    ClientData.Character.Age2 = percentage;
                }

                RefreshAging();
            };
            subMenu.AddItem(agingItem);


            // === Complexion ===
            var complexionItem = new NativeUI.UIMenuListItem("Skin Complexion", Variants.Complexion, ClientData.Character.Complexion1,
                "Make changes to your Appearance.");
            complexionItem.OnListChanged += (sender, index) =>
            {
                ClientData.Character.Complexion1 = index;
                if (index == 0)
                {
                    complexionItem.RemovePanelAt(0);
                    ClientData.Character.Complexion2 = 0;
                }
                else
                {
                    if (complexionItem.Panels.Count == 0)
                    {
                        var percentageItem = new NativeUI.UIMenuPercentagePanel("Opacity", "0%", "100%")
                        {
                            Percentage = ClientData.Character.Age2
                        };
                        complexionItem.AddPanel(percentageItem);
                    }

                    var percentagePanel = sender.Panels[0] as NativeUI.UIMenuPercentagePanel;
                    var percentage = percentagePanel?.Percentage ?? 1.0f;
                    ClientData.Character.Complexion2 = percentage;
                }

                RefreshComplexion();
            };
            subMenu.AddItem(complexionItem);


            // === Molefreckle ===
            var molefreckleItem = new NativeUI.UIMenuListItem("Moles & Freckles", Variants.Molefreckle, ClientData.Character.Moles1,
                "Make changes to your Appearance.");
            molefreckleItem.OnListChanged += (sender, index) =>
            {
                ClientData.Character.Moles1 = index;
                if (index == 0)
                {
                    molefreckleItem.RemovePanelAt(0);
                    ClientData.Character.Moles2 = 0;
                }
                else
                {
                    if (molefreckleItem.Panels.Count == 0)
                    {
                        var percentageItem = new NativeUI.UIMenuPercentagePanel("Opacity", "0%", "100%")
                        {
                            Percentage = ClientData.Character.Age2
                        };
                        molefreckleItem.AddPanel(percentageItem);
                    }

                    var percentagePanel = sender.Panels[0] as NativeUI.UIMenuPercentagePanel;
                    var percentage = percentagePanel?.Percentage ?? 1.0f;
                    ClientData.Character.Moles2 = percentage;
                }

                RefreshMoleFreckle();
            };
            subMenu.AddItem(molefreckleItem);
            

            // === SunDamage ===
            var sunDamageItem = new NativeUI.UIMenuListItem("Skin Damage", Variants.SunDamage, ClientData.Character.Sun1,
                "Make changes to your Appearance.");
            sunDamageItem.OnListChanged += (sender, index) =>
            {
                ClientData.Character.Sun1 = index;
                if (index == 0)
                {
                    sunDamageItem.RemovePanelAt(0);
                    ClientData.Character.Sun2 = 0;
                }
                else
                {
                    if (sunDamageItem.Panels.Count == 0)
                    {
                        var percentageItem = new NativeUI.UIMenuPercentagePanel("Opacity", "0%", "100%")
                        {
                            Percentage = ClientData.Character.Sun2
                        };
                        sunDamageItem.AddPanel(percentageItem);
                    }

                    var percentagePanel = sender.Panels[0] as NativeUI.UIMenuPercentagePanel;
                    var percentage = percentagePanel?.Percentage ?? 1.0f;
                    ClientData.Character.Sun2 = percentage;
                }

                RefreshSunDamage();
            };
            subMenu.AddItem(sunDamageItem);

            // === EyeColor ===
            var eyeColorItem = new NativeUI.UIMenuListItem("Eye Color", Variants.EyeColorNames, ClientData.Character.EyeColor,
                "Make changes to your Appearance.");
            eyeColorItem.OnListChanged += (sender, index) =>
            {
                ClientData.Character.EyeColor = index;
                RefreshEyeColor();
            };
            subMenu.AddItem(eyeColorItem);


            // === Makeup ===
            var makeupItem =
                new NativeUI.UIMenuListItem("Makeup", Variants.Makeups, ClientData.Character.Makeup1, "Make changes to your Appearance.");
            makeupItem.OnListChanged += (sender, index) =>
            {
                ClientData.Character.Makeup1 = sender.Index;
                if (index == 0)
                {
                    makeupItem.RemovePanelAt(0);
                    makeupItem.RemovePanelAt(0);
                    ClientData.Character.Makeup2 = 0;
                    ClientData.Character.Makeup3 = 0;
                }
                else
                {
                    if (sender.Panels.Count == 0)
                    {
                        var makeupColorPanel = new NativeUI.UIMenuColorPanel("Color", NativeUI.UIMenuColorPanel.ColorPanelType.Makeup);
                        var makeupPercentageItem = new NativeUI.UIMenuPercentagePanel("Opacity", "0%", "100%")
                        {
                            Percentage = ClientData.Character.Makeup2
                        };
                        makeupItem.AddPanel(makeupColorPanel);
                        makeupItem.AddPanel(makeupPercentageItem);
                    }

                    var colorPanel = sender.Panels[0] as NativeUI.UIMenuColorPanel;
                    var percentagePanel = sender.Panels[1] as NativeUI.UIMenuPercentagePanel;
                    var percentage = percentagePanel?.Percentage ?? 1.0f;
                    var color = colorPanel?.CurrentSelection ?? 1;
                    ClientData.Character.Makeup2 = percentage;
                    ClientData.Character.Makeup3 = color;
                }

                RefreshMakeup();
            };
            subMenu.AddItem(makeupItem);


            // === Blush ===
            var blushItem =
                new NativeUI.UIMenuListItem("Blusher", Variants.Blushes, ClientData.Character.Blush1, "Make changes to your Appearance.");
            blushItem.OnListChanged += (sender, index) =>
            {
                ClientData.Character.Blush1 = sender.Index;
                if (index == 0)
                {
                    blushItem.RemovePanelAt(0);
                    blushItem.RemovePanelAt(0);
                    ClientData.Character.Blush2 = 0;
                    ClientData.Character.Blush3 = 0;
                }
                else
                {
                    if (sender.Panels.Count == 0)
                    {
                        var blushColorPanel = new NativeUI.UIMenuColorPanel("Color", NativeUI.UIMenuColorPanel.ColorPanelType.Makeup);
                        var blushPercentageItem = new NativeUI.UIMenuPercentagePanel("Opacity", "0%", "100%")
                        {
                            Percentage = ClientData.Character.Blush2
                        };
                        blushItem.AddPanel(blushColorPanel);
                        blushItem.AddPanel(blushPercentageItem);
                    }

                    var colorPanel = sender.Panels[0] as NativeUI.UIMenuColorPanel;
                    var percentagePanel = sender.Panels[1] as NativeUI.UIMenuPercentagePanel;
                    var percentage = percentagePanel?.Percentage ?? 1.0f;
                    var color = colorPanel?.CurrentSelection ?? 1;
                    ClientData.Character.Blush2 = percentage;
                    ClientData.Character.Blush3 = color;
                }

                RefreshBlush();
            };
            subMenu.AddItem(blushItem);


            // === Lipstick ===
            var lipstickItem =
                new NativeUI.UIMenuListItem("Lipstick", Variants.Lipsticks, ClientData.Character.Lipstick1,
                    "Make changes to your Appearance.");
            lipstickItem.OnListChanged += (sender, index) =>
            {
                ClientData.Character.Lipstick1 = sender.Index;
                if (index == 0)
                {
                    lipstickItem.RemovePanelAt(0);
                    lipstickItem.RemovePanelAt(0);
                    ClientData.Character.Lipstick2 = 0;
                    ClientData.Character.Lipstick3 = 0;
                }
                else
                {
                    if (sender.Panels.Count == 0)
                    {
                        var makeupColorPanel =
                            new NativeUI.UIMenuColorPanel("Color", NativeUI.UIMenuColorPanel.ColorPanelType.Makeup);
                        var makeupPercentageItem = new NativeUI.UIMenuPercentagePanel("Opacity", "0%", "100%")
                        {
                            Percentage = ClientData.Character.Eyebrows2
                        };
                        lipstickItem.AddPanel(makeupColorPanel);
                        lipstickItem.AddPanel(makeupPercentageItem);
                    }

                    var colorPanel = sender.Panels[0] as NativeUI.UIMenuColorPanel;
                    var percentagePanel = sender.Panels[1] as NativeUI.UIMenuPercentagePanel;
                    var percentage = percentagePanel?.Percentage ?? 1.0f;
                    var color = colorPanel?.CurrentSelection ?? 1;
                    ClientData.Character.Lipstick2 = percentage;
                    ClientData.Character.Lipstick3 = color;
                }

                RefreshLipstick();
            };
            subMenu.AddItem(lipstickItem);


            subMenu.OnMenuStateChanged += (oldMenu, newMenu, state) =>
            {
                if (state == NativeUI.MenuState.ChangeForward)
                {
                    SwitchCamera(_faceCamera, _bodyCamera);
                    if (ClientData.Character.Gender == MaleGender)
                    {
                        hairItem.Items = Variants.MaleHairNames;
                        hairItem.Index = ClientData.Character.Hair;
                        beardItem.Enabled = true;
                    }
                    else if (ClientData.Character.Gender == FemaleGender)
                    {
                        hairItem.Items = Variants.FemaleHairNames;
                        hairItem.Index = ClientData.Character.Hair;
                        ClientData.Character.Beard = 0;
                        ClientData.Character.Beard2 = 0;
                        ClientData.Character.Beard3 = 0;
                        beardItem.Index = ClientData.Character.Beard;
                        beardItem.RemovePanelAt(0);
                        beardItem.RemovePanelAt(0);
                        beardItem.Enabled = false;
                    }
                }
                else if (state == NativeUI.MenuState.ChangeBackward)
                {
                    SwitchCamera(_bodyCamera, _faceCamera);
                }
            };
        }
        
        private void AddFaceShapeSlider(NativeUI.UIMenu subMenu, string text, string description, float value,
            NativeUI.ItemSliderEvent onSliderChanged)
        {
            var slider = new NativeUI.UIMenuSliderItem(text, description, true)
            {
                Value = (int)Math.MapRange(value, -1, 1, 0, 100)
            };
            subMenu.AddItem(slider);
            slider.OnSliderChanged += onSliderChanged;
        }
        private void ChangeFaceShapeValue(float value, out float charValue, int faceFeatureIndex)
        {
            var newValue = Math.MapRange(value, 0, 100, -1, 1);
            charValue = newValue;
            API.SetPedFaceFeature(API.PlayerPedId(), faceFeatureIndex, charValue);
        }
    }
}