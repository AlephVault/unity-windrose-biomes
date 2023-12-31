using System;
using AlephVault.Unity.Support.Utils;
using AlephVault.Unity.WindRose.Authoring.Behaviours.Entities.Objects.Strategies;
using AlephVault.Unity.WindRose.Authoring.Behaviours.Entities.Objects.Strategies.Base;
using AlephVault.Unity.WindRose.Authoring.Behaviours.World.Layers.Objects.ObjectsManagementStrategies.Base;
using AlephVault.Unity.WindRose.Biomes.Authoring.Behaviours.World.Layers.Objects.ObjectsManagementStrategies;
using AlephVault.Unity.WindRose.Biomes.Authoring.ScriptableObjects.Tiles;
using AlephVault.Unity.WindRose.Biomes.Types;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace AlephVault.Unity.WindRose.Biomes
{
    namespace Authoring
    {
        namespace Behaviours
        {
            namespace Entities.Objects
            {
                namespace Strategies
                {
                    /// <summary>
                    ///   This strategy is just the counterpart of <see cref="BiomeObjectsManagementStrategy"/>.
                    /// </summary>
                    [RequireComponent(typeof(LayoutObjectStrategy))]
                    public class BiomeObjectStrategy : ObjectStrategy
                    {
                        /// <summary>
                        ///   The related layout strategy.
                        /// </summary>
                        public LayoutObjectStrategy LayoutStrategy { get; private set; }
                        
                        #if UNITY_EDITOR
                        [CustomEditor(typeof(BiomeObjectStrategy))]
                        public class BiomeObjectStrategyEditor : Editor
                        {
                            // The biome tile strategy being edited.
                            private BiomeObjectStrategy component;

                            // The biome indices.
                            private int[] indices;
                            
                            // The biome captions.
                            private string[] captions;

                            private void OnEnable()
                            {
                                component = (BiomeObjectStrategy)target;
                                RefreshOptions();
                            }

                            private void RefreshOptions()
                            {
                                if (component.biomeSet != null)
                                {
                                    int index = 0;
                                    indices = new int[component.biomeSet.Count];
                                    captions = new string[component.biomeSet.Count];
                                    foreach (Tuple<string, string> entry in component.biomeSet)
                                    {
                                        indices[index] = index;
                                        captions[index] = $"{entry.Item2} ({entry.Item1})";
                                        index++;
                                    }
                                }
                                else
                                {
                                    indices = null;
                                    captions = null;
                                }
                            }

                            public override void OnInspectorGUI()
                            {
                                if (!component) return;
                                
                                component.biomeSet = (BiomeSet) EditorGUILayout.ObjectField(
                                    "Biome Set", component.biomeSet,
                                    typeof(BiomeSet), component
                                );
                                RefreshOptions();
                                if (component.biomeSet)
                                {
                                    component.biome = (byte)Values.Clamp(
                                        0, EditorGUILayout.IntPopup(
                                            "Biome", component.biome, captions, indices
                                        ),
                                        component.biomeSet.Count - 1
                                    );
                                }
                                else
                                {
                                    EditorGUILayout.HelpBox(
                                        "Please select a biome set. Otherwise, this strategy won't work",
                                        MessageType.Warning
                                    );
                                }
                                
                                EditorUtility.SetDirty(component);
                            }
                        }
                        #endif
                        
                        /// <summary>
                        ///   The counterpart type is <see cref="BiomeObjectsManagementStrategy"/>.
                        /// </summary>
                        protected override Type GetCounterpartType()
                        {
                            return typeof(BiomeObjectsManagementStrategy);
                        }
                        
                        /// <summary>
                        ///   The biome set this strategy relates to.
                        /// </summary>
                        [SerializeField]
                        internal BiomeSet biomeSet;

                        /// <summary>
                        ///   The current biome for this object.
                        /// </summary>
                        [SerializeField]
                        private byte biome = 0;

                        /// <summary>
                        ///   See <see cref="biome"/>.
                        /// </summary>
                        public byte Biome
                        {
                            get => biome;
                            set
                            {
                                if (value >= biomeSet.Count)
                                {
                                    throw new IndexOutOfRangeException(
                                        "The new biome index is not valid"
                                    );
                                }

                                byte oldBiome = biome;
                                biome = value;
                                PropertyWasUpdated("biome", oldBiome, biome);
                            }
                        }

                        protected override void Awake()
                        {
                            base.Awake();
                            LayoutStrategy = GetComponent<LayoutObjectStrategy>();

                            if (biomeSet == null)
                            {
                                Destroy(gameObject);
                                throw new MissingBiomeSetException(
                                    "A biome set must be added to this object strategy"
                                );
                            }

                            if (biome >= biomeSet.Count)
                            {
                                Destroy(gameObject);
                                throw new IndexOutOfRangeException(
                                    "The default biome index is not valid"
                                );
                            }
                        }
                    }
                }
            }
        }
    }
}