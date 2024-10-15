import Knight from "./knight";

interface KnightData extends Omit<Knight, "id" | "age" | "experience" | "attack"> { }

export default KnightData;