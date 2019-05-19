using System;
using System.Collections.Generic;
using Models.Hashing;
using Server.Models.Cloning;

namespace Models.Client
{
    public class ConfigModel:IDeepCopyable<ConfigModel>
    {
        public ConfigModel(){
            RowBonus=2;
            ColBonus=7;
            AllOfKindBonus=10;
            FloorPenalties=new int[]{-1,-1,-2,-2,-2,-3,-3};
            wallLayoutToMatch=new TileType[][]{
                new TileType[]{TileType.Blue,TileType.Yellow,TileType.Red,TileType.Black,TileType.White},
                new TileType[]{TileType.White,TileType.Blue,TileType.Yellow,TileType.Red,TileType.Black},
                new TileType[]{TileType.Black,TileType.White,TileType.Blue,TileType.Yellow,TileType.Red},
                new TileType[]{TileType.Red,TileType.Black,TileType.White,TileType.Blue,TileType.Yellow},
                new TileType[]{TileType.Yellow,TileType.Red,TileType.Black,TileType.White,TileType.Blue},
            };
        }
        public int[] FloorPenalties {get;set;}
        //I wish C# has semi-auto props where you could use a keyword for the backing field
        private TileType[][] wallLayoutToMatch; public TileType[][] WallLayoutToMatch {
            get=>wallLayoutToMatch;
            set=>wallLayoutToMatch=validateWall(value);
        }
        public int RowBonus{get;set;}
        public int ColBonus{get;set;}
        public int AllOfKindBonus{get;set;}

        //constraint of only one type per row is fundamental to the game
        //because of the way patternlines must be all the same color
        private TileType[][] validateWall(TileType[][] value)
        {
            int size=Enum.GetNames(typeof(TileType)).Length;
            bool[] containsMap=new bool[size];
            for(int r=0;r<value.Length;r++){
                containsMap=new bool[size];
                if(value[r].Length!=value[0].Length) {
                    throw new ArgumentException("wall matrixes cannot be jagged");
                }
                for(int c=0;c<value[r].Length;c++){
                    if(containsMap[(int)value[r][c]]){
                        throw new ArgumentException(
                            $"row {r} has a second tile of type {value[r][c]} at col {c}"
                        );
                    } 
                }
            }
            return value;
        }
        public override int GetHashCode(){
            int hash=17;
            ModelHashUtils.CombineHash(ref hash,ModelHashUtils.HashOfEnumerable(FloorPenalties));
            ModelHashUtils.CombineHash(ref hash,ModelHashUtils.HashOfEnumerableOfEnumerable(WallLayoutToMatch));
            ModelHashUtils.CombineHash(ref hash,RowBonus);
            ModelHashUtils.CombineHash(ref hash,ColBonus);
            ModelHashUtils.CombineHash(ref hash,AllOfKindBonus);
            return hash;
        }
        public ConfigModel DeepCopy()=> new ConfigModel{
            FloorPenalties=DeepCopyStruct<int>.Array(this.FloorPenalties),
            WallLayoutToMatch=DeepCopyStruct<TileType>.ArrayOfArray(this.WallLayoutToMatch),
            RowBonus=RowBonus,
            ColBonus=ColBonus,
            AllOfKindBonus=AllOfKindBonus
        };
    }
}
