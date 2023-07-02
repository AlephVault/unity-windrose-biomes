using System;
using AlephVault.Unity.WindRose.Authoring.Behaviours.Entities.Objects.Strategies;
using AlephVault.Unity.WindRose.Authoring.Behaviours.World.Layers.Objects;
using AlephVault.Unity.WindRose.Authoring.Behaviours.World.Layers.Objects.ObjectsManagementStrategies;
using AlephVault.Unity.WindRose.Authoring.Behaviours.World.Layers.Objects.ObjectsManagementStrategies.Base;
using AlephVault.Unity.WindRose.Authoring.Behaviours.World.Layers.Objects.ObjectsManagementStrategies.Solidness;
using AlephVault.Unity.WindRose.Types;
using UnityEngine;


namespace AlephVault.Unity.WindRose.Biomes
{
    using Types;
    namespace Authoring
    {
        namespace Behaviours
        {
            using Entities.Objects.Strategies;

            namespace World
            {
                namespace Layers
                {
                    namespace Objects
                    {
                        namespace ObjectsManagementStrategies
                        {
                            /// <summary>
                            ///   <para>
                            ///     Combines the power of <see cref="LayoutObjectsManagementStrategy"/>
                            ///       which forbids walking through blocked cells, <see cref="SolidnessObjectsManagementStrategy"/>
                            ///       which forbids solid objects walking through occupied cells, and finally
                            ///       <see cref="BiomeObjectsManagementStrategy"/> which forbids non-biomatic
                            ///       movement for the object.
                            ///   </para>
                            ///   <para>
                            ///     Its counterpart is <see cref="BiomeAwareSimpleObjectStrategy"/>.
                            ///   </para> 
                            /// </summary>
                            [RequireComponent(typeof(SolidnessObjectsManagementStrategy))]
                            [RequireComponent(typeof(BiomeObjectsManagementStrategy))]
                            public class BiomeAwareSimpleObjectsManagementStrategy : ObjectsManagementStrategy
                            {
                                /// <summary>
                                ///   The related solidness strategy.
                                /// </summary>
                                public SolidnessObjectsManagementStrategy SolidnessStrategy  { get; private set; }
                                
                                /// <summary>
                                ///   The related biomes strategy.
                                /// </summary>
                                public BiomeObjectsManagementStrategy BiomeStrategy { get; private set; }

                                protected override void Awake()
                                {
                                    base.Awake();
                                    SolidnessStrategy = GetComponent<SolidnessObjectsManagementStrategy>();
                                    BiomeStrategy = GetComponent<BiomeObjectsManagementStrategy>();
                                }

                                protected override Type GetCounterpartType()
                                {
                                    return typeof(BiomeAwareSimpleObjectStrategy);
                                }

                                public override bool CanAllocateMovement(ObjectStrategy strategy, ObjectsManagementStrategyHolder.Status status, Direction direction, bool continued)
                                {
                                    BiomeAwareSimpleObjectStrategy basStrategy = (BiomeAwareSimpleObjectStrategy) strategy;
                                    return BiomeStrategy.CanAllocateMovement(
                                        basStrategy.BiomeStrategy, status, direction, continued
                                    ) && SolidnessStrategy.CanAllocateMovement(
                                        basStrategy.SolidnessStrategy, status, direction, continued
                                    );
                                }

                                public override bool CanClearMovement(ObjectStrategy strategy, ObjectsManagementStrategyHolder.Status status)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
