const getType = (a) =>
    ({}).toString.call(a);

const mapObject = (a, f) =>
    Object.keys(a).map((key) => f(key, a[key]));

export const ofString = (String, Number, Object, Array, Boolean, Null, Ok, Err) => {
    const mapper = (key, value) => [key, makeJson(value)];

    const makeJson = (a) => {
        const type = getType(a);

        switch (type) {
        case "[object Object]":
            return Object(mapObject(a, mapper));
        case "[object Array]":
            return Array(a.map(makeJson));
        case "[object Number]":
            return Number(a);
        case "[object String]":
            return String(a);
        case "[object Boolean]":
            return Boolean(a);
        case "[object Null]":
            return Null;
        default:
            throw new Error("Unknown type: " + type);
        }
    };

    return (input) => {
        try {
            return Ok(makeJson(JSON.parse(input)));

        } catch (e) {
            return Err(e);
        }
    };
};
