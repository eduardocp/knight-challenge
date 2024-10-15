import Attributes from "./attributes";
import Weapon from "./weapons";

interface Knight {
    id: string;
    name: string;
    nickName: string;
    birthday: string;
    age: number;
    weapons: Weapon[];
    attributes: Attributes;
    keyAttribute: string;
    experience: number;
    attack: number;
}

export default Knight;