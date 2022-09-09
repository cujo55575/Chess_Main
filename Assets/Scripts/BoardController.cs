using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ChessNetWork
{
    public class BoardController : MonoBehaviour
    {
        [Header("-----Board Related-----")]
        [SerializeField] private SO_BoardTheme boardTheme;
        [SerializeField] private Vector3 boardCenter = Vector3.zero;
        [HideInInspector] public GameObject[,] Tiles;

        [Header("-----Pieces Related-----")]
        [SerializeField] private SO_ChessPieces blackPieces;
        [SerializeField] private SO_ChessPieces whitePieces;

        [Header("-----UI Related-----")]
        [SerializeField] private GameObject obj_mainUI;
        [SerializeField] private GameObject obj_whiteVictory;
        [SerializeField] private GameObject obj_blackVictory;

        private ChessPiece[,] chessPieces;
        private ChessPiece currentDragPiece;
        private SpecialMove specialMoves;
        private List<ChessPiece> deadWhitePieces = new List<ChessPiece>();
        private List<ChessPiece> deadBlackPieces = new List<ChessPiece>();
        private List<Vector2Int> availableMoves = new List<Vector2Int>();
        private List<Vector2Int[]> moveList = new List<Vector2Int[]>();
        private float tileSize = 1.0f;
        private float deathSize = 1f;
        private float deadSpacing = 0.55f;
        private const int xTileCount = 8;
        private const int yTileCount = 8;
        private const string fileNames = "abcdefgh";
        private Shader squareShader;
        private Camera mainCamera;
        private Vector2Int currentHover;
        private Vector3 bounds;
        private bool isWhiteTurn;
        public static BoardController Instance;
        private void Awake()
        {
            Instance = this;
            squareShader = Shader.Find("Unlit/Color");

            isWhiteTurn = true;

            obj_mainUI.SetActive(false);
            obj_whiteVictory.SetActive(false);
            obj_blackVictory.SetActive(false);


        }
        public void ShowChessBoard()
        {
            CreateBoard(tileSize, xTileCount, yTileCount);

            SpawningPieces();
            PositionAllPieces();
        }
        private void Update()
        {
            #region hovering
            if (!mainCamera)
            {
                mainCamera = Camera.main;
                return;
            }

            RaycastHit info;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out info, 100, LayerMask.GetMask("Tile", "Hover", "HighLight")))
            {
                Vector2Int hitPosition = LookupTileIndex(info.transform.gameObject);

                // if (currentHover == -Vector2Int.one)
                // {
                //     currentHover = hitPosition;
                //     Tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
                // }
                // if (currentHover != hitPosition)
                // {
                //     Tiles[currentHover.x, currentHover.y].layer = (ContainsValidMove(ref availableMoves, currentHover)) ?
                //     LayerMask.NameToLayer("HighLight") : LayerMask.NameToLayer("Tile");

                //     currentHover = hitPosition;
                //     Tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
                // }
                if (Input.GetMouseButtonDown(0))
                {
                    if (chessPieces[hitPosition.x, hitPosition.y] != null)
                    {
                        if ((chessPieces[hitPosition.x, hitPosition.y].team == 0 && isWhiteTurn) ||
                        (chessPieces[hitPosition.x, hitPosition.y].team == 1 && !isWhiteTurn))
                        {
                            currentDragPiece = chessPieces[hitPosition.x, hitPosition.y];
                            availableMoves = currentDragPiece.GetAvailableMoves(ref chessPieces, xTileCount, yTileCount);

                            specialMoves = currentDragPiece.GetSpecialMoves(ref chessPieces, ref moveList, ref availableMoves);

                            PreventCheck();
                            HighLightTiles();
                        }
                    }
                }
                if (currentDragPiece && Input.GetMouseButtonUp(0))
                {
                    Vector2Int prevPos = new Vector2Int(currentDragPiece.xIndex, currentDragPiece.yIndex);
                    bool validMove = MoveTo(currentDragPiece, hitPosition.x, hitPosition.y);
                    if (!validMove)
                    {
                        currentDragPiece.SetPos(GetTileCenter(prevPos.x, prevPos.y));
                    }
                    currentDragPiece = null;
                    RemoveHighLightTiles();
                }
            }
            else
            {
                // if (currentHover != -Vector2Int.one)
                // {
                //     Tiles[currentHover.x, currentHover.y].layer = (ContainsValidMove(ref availableMoves, currentHover)) ?
                //     LayerMask.NameToLayer("HighLight") : LayerMask.NameToLayer("Tile");
                //     currentHover = -Vector2Int.one;
                // }
                if (currentDragPiece && Input.GetMouseButtonUp(0))
                {
                    currentDragPiece.SetPos(GetTileCenter(currentDragPiece.xIndex, currentDragPiece.yIndex));
                    currentDragPiece = null;
                    RemoveHighLightTiles();
                }
            }
            if (currentDragPiece)
            {
                Plane horizontalPlane = new Plane(Vector3.forward, Vector3.up);
                float distance = 0f;
                if (horizontalPlane.Raycast(ray, out distance))
                    currentDragPiece.SetPos(ray.GetPoint(distance), true);
            }
            #endregion

        }
        #region Generate Board
        private void CreateBoard(float size, int _xTileCount, int _yTileCount)
        {
            bounds = new Vector3((_xTileCount / 2) * size, (_yTileCount / 2) * size, 0) + boardCenter;

            Tiles = new GameObject[_xTileCount, _yTileCount];
            for (int x = 0; x < _xTileCount; x++)
            {
                for (int y = 0; y < _yTileCount; y++)
                {
                    Tiles[x, y] = GenerateTile(size, x, y);
                }
            }
        }
        private GameObject GenerateTile(float size, int x, int y)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Quad);
            go.name = SquareNameFromPosition(x, y);
            go.transform.SetParent(transform);
            go.transform.position = PositionFromCoord(x, y, 0);
            go.layer = LayerMask.NameToLayer("Tile");

            Material squareMaterial = new Material(squareShader);
            go.GetComponent<MeshRenderer>().material = squareMaterial;
            go.GetComponent<MeshRenderer>().material.color = TileColor(x, y);

            return go;
        }
        private Vector2Int LookupTileIndex(GameObject hitInfo)
        {
            for (int x = 0; x < xTileCount; x++)
            {
                for (int y = 0; y < xTileCount; y++)
                {
                    if (Tiles[x, y] == hitInfo) return new Vector2Int(x, y);
                }
            }
            return -Vector2Int.one;
        }
        private string SquareNameFromPosition(int fileIndex, int rankIndex)
        {
            return fileNames[fileIndex] + "" + (rankIndex + 1);
        }
        private Vector3 PositionFromCoord(int x, int y, float depth = 0)
        {
            return new Vector3(-3.5f + x, -3.5f + y, depth);
            // return new Vector3(-3.5f + 7 - x, 7 - y - 3.5f, depth);
        }
        private Color TileColor(int x, int y)
        {
            bool IsLightSquare = (x + y) % 2 != 0;
            return (IsLightSquare) ? boardTheme.lightSquares.normal : boardTheme.darkSquares.normal;
        }
        #endregion
        #region Spawing Pieces
        private void SpawningPieces()
        {
            chessPieces = new ChessPiece[xTileCount, yTileCount];
            //White Side
            chessPieces[0, 0] = SpawningSinglePiece(whitePieces, ChessPiecesType.Rook);
            chessPieces[1, 0] = SpawningSinglePiece(whitePieces, ChessPiecesType.Knight);
            chessPieces[2, 0] = SpawningSinglePiece(whitePieces, ChessPiecesType.Bishop);
            chessPieces[3, 0] = SpawningSinglePiece(whitePieces, ChessPiecesType.Queen);
            chessPieces[4, 0] = SpawningSinglePiece(whitePieces, ChessPiecesType.King);
            chessPieces[5, 0] = SpawningSinglePiece(whitePieces, ChessPiecesType.Bishop);
            chessPieces[6, 0] = SpawningSinglePiece(whitePieces, ChessPiecesType.Knight);
            chessPieces[7, 0] = SpawningSinglePiece(whitePieces, ChessPiecesType.Rook);
            for (int i = 0; i < xTileCount; i++)
                chessPieces[i, 1] = SpawningSinglePiece(whitePieces, ChessPiecesType.Pawn);

            //Black Side
            chessPieces[0, 7] = SpawningSinglePiece(blackPieces, ChessPiecesType.Rook);
            chessPieces[1, 7] = SpawningSinglePiece(blackPieces, ChessPiecesType.Knight);
            chessPieces[2, 7] = SpawningSinglePiece(blackPieces, ChessPiecesType.Bishop);
            chessPieces[3, 7] = SpawningSinglePiece(blackPieces, ChessPiecesType.Queen);
            chessPieces[4, 7] = SpawningSinglePiece(blackPieces, ChessPiecesType.King);
            chessPieces[5, 7] = SpawningSinglePiece(blackPieces, ChessPiecesType.Bishop);
            chessPieces[6, 7] = SpawningSinglePiece(blackPieces, ChessPiecesType.Knight);
            chessPieces[7, 7] = SpawningSinglePiece(blackPieces, ChessPiecesType.Rook);
            for (int i = 0; i < xTileCount; i++)
                chessPieces[i, 6] = SpawningSinglePiece(blackPieces, ChessPiecesType.Pawn);

        }
        private ChessPiece SpawningSinglePiece(SO_ChessPieces so_ChessPieces, ChessPiecesType type)
        {
            GameObject[] prefabs = so_ChessPieces.chessPieces;
            ChessPiece cp = Instantiate(prefabs[(int)type - 1], transform).GetComponent<ChessPiece>();
            cp.gameObject.name = prefabs[(int)type - 1].name;
            cp.pieceType = type;
            cp.team = (int)so_ChessPieces.chessPieceType;
            return cp;
        }
        private void PositionAllPieces()
        {
            for (int x = 0; x < xTileCount; x++)
            {
                for (int y = 0; y < yTileCount; y++)
                {
                    if (chessPieces[x, y] != null)
                    {
                        PositionSinglPiece(x, y, true);
                    }
                }
            }
        }
        private void PositionSinglPiece(int x, int y, bool force = false)
        {
            chessPieces[x, y].xIndex = x;
            chessPieces[x, y].yIndex = y;
            chessPieces[x, y].SetPos(GetTileCenter(x, y), force);
        }
        private Vector3 GetTileCenter(int x, int y)
        {
            return new Vector3(x * tileSize, y * tileSize, -1) - bounds + new Vector3(tileSize / 2, tileSize / 2, 0);
        }

        #endregion
        #region Moving Pieces
        private bool MoveTo(ChessPiece currentPiece, int x, int y)
        {
            if (!ContainsValidMove(ref availableMoves, new Vector2Int(x, y))) return false;

            Vector2Int previousPos = new Vector2Int(currentPiece.xIndex, currentPiece.yIndex);

            if (chessPieces[x, y] != null)
            {
                ChessPiece otherPiece = chessPieces[x, y];
                if (currentPiece.team == otherPiece.team)
                    return false;

                if (otherPiece.team == 0)
                {
                    if (otherPiece.pieceType == ChessPiecesType.King)
                        CheckMate(1);
                    deadWhitePieces.Add(otherPiece);
                    otherPiece.SetScale(Vector3.one * deathSize);
                    otherPiece.SetPos(new Vector3(8 * tileSize, -1 * tileSize, 0) - bounds +
                    new Vector3(tileSize / 2, tileSize / 2, 0) + (Vector3.up * deadSpacing) * deadWhitePieces.Count);
                }
                else
                {
                    if (otherPiece.pieceType == ChessPiecesType.King)
                        CheckMate(0);

                    deadBlackPieces.Add(otherPiece);
                    otherPiece.SetScale(Vector3.one * deathSize);
                    otherPiece.SetPos(new Vector3(-1 * tileSize, 8 * tileSize, 0) - bounds +
                    new Vector3(tileSize / 2, tileSize / 2, 0) + (Vector3.down * deadSpacing) * deadBlackPieces.Count);
                }
            }

            chessPieces[x, y] = currentPiece;
            chessPieces[previousPos.x, previousPos.y] = null;

            PositionSinglPiece(x, y);

            isWhiteTurn = !isWhiteTurn;
            moveList.Add(new Vector2Int[] { previousPos, new Vector2Int(x, y) });

            ProcessSpecialMove();

            if (CheckForCheckmate()) CheckMate(currentPiece.team);

            return true;
        }
        #endregion
        #region HighLigting Tiles
        private void HighLightTiles()
        {
            for (int i = 0; i < availableMoves.Count; i++)
            {
                // Tiles[availableMoves[i].x, availableMoves[i].y].layer = LayerMask.NameToLayer("HighLight");

                Tiles[availableMoves[i].x, availableMoves[i].y].GetComponent<MeshRenderer>().material.color = boardTheme.lightSquares.moveToHighlight;
            }
        }

        private void RemoveHighLightTiles()
        {
            for (int i = 0; i < availableMoves.Count; i++)
            {
                // Tiles[availableMoves[i].x, availableMoves[i].y].layer = LayerMask.NameToLayer("Tile");
                Tiles[availableMoves[i].x, availableMoves[i].y].GetComponent<MeshRenderer>().material.color
                = TileColor(availableMoves[i].x, availableMoves[i].y);
            }
            availableMoves.Clear();
        }
        #endregion
        #region CheckMate
        private void CheckMate(int team)
        {
            DisplayVictory(team);
        }
        private void DisplayVictory(int winningTeam)
        {
            obj_mainUI.SetActive(true);
            obj_whiteVictory.SetActive(winningTeam == 0 ? true : false);
            obj_blackVictory.SetActive(winningTeam == 0 ? false : true);
        }
        public void OnResetButton()
        {
            #region UI
            obj_mainUI.SetActive(false);
            obj_whiteVictory.SetActive(false);
            obj_blackVictory.SetActive(false);
            #endregion
            #region Clearing Out List
            currentDragPiece = null;
            availableMoves.Clear();
            moveList.Clear();
            for (int x = 0; x < xTileCount; x++)
            {
                for (int y = 0; y < yTileCount; y++)
                {
                    if (chessPieces[x, y] != null)
                        Destroy(chessPieces[x, y].gameObject);
                    chessPieces[x, y] = null;
                }
            }
            for (int i = 0; i < deadWhitePieces.Count; i++)
            {
                Destroy(deadWhitePieces[i].gameObject);
            }
            for (int i = 0; i < deadBlackPieces.Count; i++)
            {
                Destroy(deadBlackPieces[i].gameObject);
            }
            deadBlackPieces.Clear();
            deadBlackPieces.Clear();
            #endregion
            #region Respawn Pieces
            SpawningPieces();
            PositionAllPieces();
            isWhiteTurn = true;
            #endregion

        }
        public void OnExitButton()
        {
            Application.Quit();
        }
        #endregion
        #region  SpecialMoves
        private void ProcessSpecialMove()
        {
            if (specialMoves == SpecialMove.EnPassant)
            {
                var newMove = moveList[moveList.Count - 1];
                ChessPiece playerPawn = chessPieces[newMove[1].x, newMove[1].y];
                var targetPawnPosition = moveList[moveList.Count - 2];
                ChessPiece enemyPawn = chessPieces[targetPawnPosition[1].x, targetPawnPosition[1].y];

                if (playerPawn.xIndex == enemyPawn.xIndex)
                {
                    if (playerPawn.yIndex == enemyPawn.yIndex - 1 || playerPawn.yIndex == enemyPawn.yIndex + 1)
                    {
                        if (enemyPawn.team == 0)
                        {
                            deadWhitePieces.Add(enemyPawn);
                            enemyPawn.SetScale(Vector3.one * deathSize);
                            enemyPawn.SetPos(new Vector3(8 * tileSize, -1 * tileSize, 0) - bounds +
                            new Vector3(tileSize / 2, tileSize / 2, 0) + (Vector3.up * deadSpacing) * deadWhitePieces.Count);
                        }
                        else
                        {
                            deadBlackPieces.Add(enemyPawn);
                            enemyPawn.SetScale(Vector3.one * deathSize);
                            enemyPawn.SetPos(new Vector3(-1 * tileSize, 8 * tileSize, 0) - bounds +
                            new Vector3(tileSize / 2, tileSize / 2, 0) + (Vector3.down * deadSpacing) * deadBlackPieces.Count);
                        }
                        chessPieces[enemyPawn.xIndex, enemyPawn.yIndex] = null;
                    }
                }
            }
        }
        #endregion
        #region Helper Methods
        private bool ContainsValidMove(ref List<Vector2Int> moves, Vector2 pos)
        {
            for (int i = 0; i < moves.Count; i++)
            {
                if (moves[i].x == pos.x && moves[i].y == pos.y)
                {
                    return true;
                }
            }
            return false;
        }

        private void PreventCheck()
        {
            ChessPiece targetKing = null;
            for (int x = 0; x < xTileCount; x++)
            {
                for (int y = 0; y < yTileCount; y++)
                {
                    if (chessPieces[x, y] != null)
                    {
                        if (chessPieces[x, y].pieceType == ChessPiecesType.King)
                        {
                            if (chessPieces[x, y].team == currentDragPiece.team)
                            {
                                targetKing = chessPieces[x, y];
                            }
                        }
                    }
                }
            }
            SimulateMoveForSinglePice(currentDragPiece, ref availableMoves, targetKing);
        }
        private void SimulateMoveForSinglePice(ChessPiece currentPiece, ref List<Vector2Int> moves, ChessPiece targetKing)
        {
            int currentX = currentPiece.xIndex;
            int currentY = currentPiece.yIndex;
            List<Vector2Int> movesToRemove = new List<Vector2Int>();

            for (int i = 0; i < moves.Count; i++)
            {
                int simX = moves[i].x;
                int simY = moves[i].y;

                Vector2Int kingPositionThisSim = new Vector2Int(
                    targetKing.xIndex, targetKing.yIndex);
                if (currentPiece.pieceType == ChessPiecesType.King)
                {
                    kingPositionThisSim = new Vector2Int(simX, simY);
                }
                ChessPiece[,] simulation = new ChessPiece[xTileCount, yTileCount];
                List<ChessPiece> simulationAttackingPieces = new List<ChessPiece>();
                for (int x = 0; x < xTileCount; x++)
                {
                    for (int y = 0; y < yTileCount; y++)
                    {
                        if (chessPieces[x, y] != null)
                        {
                            simulation[x, y] = chessPieces[x, y];
                            if (simulation[x, y].team != currentPiece.team)
                                simulationAttackingPieces.Add(simulation[x, y]);
                        }
                    }
                }
                simulation[currentX, currentY] = null;
                currentPiece.xIndex = simX;
                currentPiece.yIndex = simY;
                simulation[simX, simY] = currentPiece;

                var deadPiece = simulationAttackingPieces.Find(c =>
                c.xIndex == simX && c.yIndex == simY);
                if (deadPiece != null)
                    simulationAttackingPieces.Remove(deadPiece);

                List<Vector2Int> simMoves = new List<Vector2Int>();
                for (int a = 0; a < simulationAttackingPieces.Count; a++)
                {
                    var pieceMoves = simulationAttackingPieces[a].GetAvailableMoves(
                        ref simulation, xTileCount, yTileCount);
                    for (int b = 0; b < pieceMoves.Count; b++)
                    {
                        simMoves.Add(pieceMoves[b]);
                    }
                }
                if (ContainsValidMove(ref simMoves, kingPositionThisSim))
                {
                    movesToRemove.Add(moves[i]);
                }
                currentPiece.xIndex = currentX;
                currentPiece.yIndex = currentY;
            }

            for (int i = 0; i < movesToRemove.Count; i++)
            {
                moves.Remove(movesToRemove[i]);
            }
        }
        private bool CheckForCheckmate()
        {
            var lastMove = moveList[moveList.Count - 1];
            int targetTeam = (chessPieces
            [lastMove[1].x, lastMove[1].y].team == 0) ? 1 : 0;

            List<ChessPiece> attackngPieces = new List<ChessPiece>();
            List<ChessPiece> defendingPieces = new List<ChessPiece>();
            ChessPiece targetKing = null;
            for (int x = 0; x < xTileCount; x++)
            {
                for (int y = 0; y < yTileCount; y++)
                {
                    if (chessPieces[x, y] != null)
                    {
                        if (chessPieces[x, y].team == targetTeam)
                        {
                            defendingPieces.Add(chessPieces[x, y]);
                            if (chessPieces[x, y].pieceType == ChessPiecesType.King)
                            {
                                targetKing = chessPieces[x, y];
                            }
                        }
                        else
                        {
                            attackngPieces.Add(chessPieces[x, y]);
                        }
                    }
                }
            }
            List<Vector2Int> currentAvailableMoves = new List<Vector2Int>();
            for (int a = 0; a < attackngPieces.Count; a++)
            {
                var pieceMoves = attackngPieces[a].GetAvailableMoves(
                    ref chessPieces, xTileCount, yTileCount);
                for (int b = 0; b < pieceMoves.Count; b++)
                {
                    currentAvailableMoves.Add(pieceMoves[b]);
                }
            }
            if (ContainsValidMove(ref currentAvailableMoves, new Vector2Int(targetKing.xIndex, targetKing.yIndex)))
            {
                for (int i = 0; i < defendingPieces.Count; i++)
                {
                    List<Vector2Int> defendingMoves = defendingPieces[i].GetAvailableMoves(
                        ref chessPieces, xTileCount, yTileCount);
                    SimulateMoveForSinglePice(defendingPieces[i], ref defendingMoves, targetKing);

                    if (defendingMoves.Count != 0) return false;
                }
                return true;
            }
            return false;
        }
        #endregion
    }
}
