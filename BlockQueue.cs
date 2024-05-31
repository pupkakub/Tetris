namespace Tetris
{
    public class BlockQueue
    {
        private readonly Block[] blocks = new Block[]
        {
            new IBlock(),
            new JBlock(),
            new LBlock(),
            new OBlock(),
            new SBlock(),
            new TBlock(),
            new ZBlock(),
        };

        private readonly Block[] pentaminoBlocks = new Block[]
        {
            new PentominoF(),
            new PentominoI(),
            new PentominoL(),
            new PentominoN(),
            new PentominoP(),
            new PentominoT(),
            new PentominoU(),
            new PentominoV(),
            new PentominoW(),
            new PentominoX(),
            new PentominoY(),
            new PentominoZ()
        };

        private readonly Random random = new Random();

        public Block NextBlock { get; private set; }

        public bool usePentamino;

        public BlockQueue(bool usePentamino)
        {
            this.usePentamino = usePentamino;
            NextBlock = RandomBlock();
        }

        private Block RandomBlock()
        {
            return usePentamino ? pentaminoBlocks[random.Next(pentaminoBlocks.Length)]: blocks[random.Next(blocks.Length)];
        }

        public Block GetAndUpdate()
        {
            if (NextBlock == null)
            {
                NextBlock = RandomBlock();
            }

            Block block = NextBlock;

            do
            {
                NextBlock = RandomBlock();
            }
            while (block == null || block.Id == NextBlock.Id);

            return block;
        }

    }
}
