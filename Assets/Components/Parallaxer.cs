using System.Collections.Generic;
using System.Linq;
using Components.UnityComponents.v1;
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

            parallaxObjectList = new List<GameObject>();

            float orthographicSize;
            screenHalfSizeWorldUnits = new Vector2(
                currentCamera.aspect * (orthographicSize = currentCamera.orthographicSize),
                orthographicSize);
            
            parallaxObjectSize = new Vector2(
                objectToParallax.GetComponent<SpriteRenderer>().bounds.size.x,
                objectToParallax.GetComponent<SpriteRenderer>().bounds.size.y);
            
            preInitiateObjects();
        }
        
        private void preInitiateObjects()
        {
            var axis = getCurrentAxis();
            
            var amountOfParallaxObjects = Mathf.Ceil(
                (screenHalfSizeWorldUnits[axis.Selected] * 2 + parallaxObjectSize[axis.Selected] * 3) /
                parallaxObjectSize[axis.Selected]);
                
            var lastAxisPosition = calculateLastPosition()[axis.Selected];
            for (var i = 0; i < amountOfParallaxObjects; i++)
            {
                var newParallaxObject = Object.Instantiate(objectToParallax, setParentTo);
                newParallaxObject.transform.position = new Vector2
                    {
                        [axis.Selected] = lastAxisPosition,
                        [axis.Unselected] = setParentTo.position[axis.Unselected]
                    };
                lastAxisPosition += parallaxObjectSize[axis.Selected] * -axis.Direction;
                parallaxObjectList.Add(newParallaxObject);
            }

            currentLastParallaxObject = parallaxObjectList.Last();
        }

        private VectorAxis getCurrentAxis()
        {
            var axis = new VectorAxis();
            if (selectMovementPostion == Direction.FromRightToLeft ||
                selectMovementPostion == Direction.FromLeftToRight)
            {
                axis.Selected = 0;
                axis.Unselected = 1;
            }
            else
            {
                axis.Selected = 1;
                axis.Unselected = 0;
            }
            
            if (selectMovementPostion == Direction.FromLeftToRight ||
                selectMovementPostion == Direction.Vertical1)
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
            var axis = getCurrentAxis();
            
            parallaxObject.transform.position =  new Vector2 
            {
                [axis.Selected] = currentLastParallaxObject.transform.position[axis.Selected] +
                    (parallaxObjectSize[axis.Selected] *
                     -axis.Direction),
                [axis.Unselected] = setParentTo.position[axis.Unselected]
            };
            
            currentLastParallaxObject = parallaxObject;
        }

        private Vector2 calculateLastPosition()
        {
            var axis = getCurrentAxis();
            
            var lastPosition = new Vector2
            {
                [axis.Selected] =
                    (screenHalfSizeWorldUnits[axis.Selected] -
                     parallaxObjectSize.x / 2) *
                    axis.Direction + gameManagerOffset[axis.Selected],
                [axis.Unselected] = setParentTo.position[axis.Unselected]
            };
            
            return lastPosition;                                            
        }

        private void parallaxCycle()
        {
            var axis = getCurrentAxis();
            
            foreach (var item in parallaxObjectList)
            {
                var positionWithOffset = 
                    item.transform.position[axis.Selected] - gameManagerOffset[axis.Selected];
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
                        currentPosition[axis.Selected] + ParallaxSpeed * Time.deltaTime * axis.Direction,
                    [axis.Unselected] = currentPosition[axis.Unselected]
                };
            }
        }
        
        public void FixedUpdateRoutine()
        {
            parallaxCycle();
        }
    }
}