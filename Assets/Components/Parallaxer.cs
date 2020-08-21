using System.Collections.Generic;
using System.Linq;
using Components.UnityComponents;
using UnityEngine;

namespace Components
{
    public struct VectorAxis
    {
        public int Selected;
        public int Unselected;
        public int Direction;
    }
    public class Parallaxer
    {
        public enum Direction
        {
                FromRightToLeft, FromLeftToRight, Vertical1, Vertical2
        }

        public float ParallaxSpeed { get; set; }

        private readonly GameObject objectToParallax;
        private readonly Direction selectMovementPostion;
        private readonly Transform setParentTo;
        
        private Vector2 parallaxObjectSize;
        private Vector2 screenHalfSizeWorldUnits;
        private MinigameManagerDefault gameManager;
        private readonly List<GameObject> parallaxObjectList;
        private GameObject currentLastParallaxObject;
        private Vector2 gameManagerOffset;

        public Parallaxer(
            GameObject objectToParallax,
            Direction selectMovementPostion,
            float parallaxSpeed,
            Camera currentCamera,
            Vector2 gameManagerOffset,
            Transform setParentTo)
        {
            this.objectToParallax = objectToParallax;
            this.selectMovementPostion = selectMovementPostion;
            this.ParallaxSpeed = parallaxSpeed;
            this.setParentTo = setParentTo;
            this.gameManagerOffset = gameManagerOffset;

            this.parallaxObjectList = new List<GameObject>();

            float orthographicSize;
            this.screenHalfSizeWorldUnits = new Vector2(
                currentCamera.aspect * (orthographicSize = currentCamera.orthographicSize),
                orthographicSize);
            
            this.parallaxObjectSize = new Vector2(
                this.objectToParallax.GetComponent<SpriteRenderer>().bounds.size.x,
                this.objectToParallax.GetComponent<SpriteRenderer>().bounds.size.y);
            
            this.preInitiateObjects();
        }
        
        private void preInitiateObjects()
        {
            var axis = this.getCurrentAxis();
            
            var amountOfParallaxObjects = Mathf.Ceil(
                (this.screenHalfSizeWorldUnits[axis.Selected] * 2 + this.parallaxObjectSize[axis.Selected] * 3) /
                this.parallaxObjectSize[axis.Selected]);
                
            var lastAxisPosition = this.calculateLastPosition()[axis.Selected];
            for (var i = 0; i < amountOfParallaxObjects; i++)
            {
                var newParallaxObject = Object.Instantiate(this.objectToParallax, this.setParentTo);
                newParallaxObject.transform.position = new Vector2
                    {
                        [axis.Selected] = lastAxisPosition,
                        [axis.Unselected] = this.setParentTo.position[axis.Unselected]
                    };
                lastAxisPosition += this.parallaxObjectSize[axis.Selected] * -axis.Direction;
                this.parallaxObjectList.Add(newParallaxObject);
            }

            this.currentLastParallaxObject = this.parallaxObjectList.Last();
        }

        private VectorAxis getCurrentAxis()
        {
            var axis = new VectorAxis();
            if (this.selectMovementPostion == Direction.FromRightToLeft ||
                this.selectMovementPostion == Direction.FromLeftToRight)
            {
                axis.Selected = 0;
                axis.Unselected = 1;
            }
            else
            {
                axis.Selected = 1;
                axis.Unselected = 0;
            }
            
            if (this.selectMovementPostion == Direction.FromLeftToRight ||
                this.selectMovementPostion == Direction.Vertical1)
            {
                axis.Direction = 1;
            }
            else
            {
                axis.Direction = -1;
            }
            
            return axis;
        }
        
        private void sendObjectToEndOfQueue(GameObject parallaxObject)
        {
            var axis = this.getCurrentAxis();
            
            parallaxObject.transform.position =  new Vector2 
            {
                [axis.Selected] = currentLastParallaxObject.transform.position[axis.Selected] +
                    (this.parallaxObjectSize[axis.Selected] *
                     -axis.Direction),
                [axis.Unselected] = this.setParentTo.position[axis.Unselected]
            };
            
            this.currentLastParallaxObject = parallaxObject;
        }

        private Vector2 calculateLastPosition()
        {
            var axis = this.getCurrentAxis();
            
            var lastPosition = new Vector2
            {
                [axis.Selected] =
                    (this.screenHalfSizeWorldUnits[axis.Selected] -
                     this.parallaxObjectSize.x / 2) *
                    axis.Direction + this.gameManagerOffset[axis.Selected],
                [axis.Unselected] = this.setParentTo.position[axis.Unselected]
            };
            
            return lastPosition;                                            
        }

        private void parallaxCycle()
        {
            var axis = this.getCurrentAxis();
            
            foreach (var item in this.parallaxObjectList)
            {
                var positionWithOffset = 
                    item.transform.position[axis.Selected] - this.gameManagerOffset[axis.Selected];
                switch (selectMovementPostion)
                {
                    default:
                    case Direction.Vertical1:
                    case Direction.FromLeftToRight: 
                        if (positionWithOffset >
                            (screenHalfSizeWorldUnits[axis.Selected] + 
                             parallaxObjectSize[axis.Selected]) * axis.Direction)
                        {
                            sendObjectToEndOfQueue(item);
                        }
                        break;
                    case Direction.Vertical2:
                    case Direction.FromRightToLeft:
                        if (positionWithOffset <
                            (screenHalfSizeWorldUnits[axis.Selected] + 
                             parallaxObjectSize[axis.Selected]) * axis.Direction)
                        {
                            sendObjectToEndOfQueue(item);
                        }
                        break;
                }

                var currentPosition = item.transform.position;
                item.transform.position = new Vector2 
                {
                    [axis.Selected] = 
                        currentPosition[axis.Selected] + this.ParallaxSpeed * Time.deltaTime * axis.Direction,
                    [axis.Unselected] = currentPosition[axis.Unselected]
                };
            }
        }
        
        public void FixedUpdateRoutine()
        {
            this.parallaxCycle();
        }
    }
}