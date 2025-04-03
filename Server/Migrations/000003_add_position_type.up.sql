DO
$$
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'character_position_type') THEN
            CREATE TYPE character_position_type AS
            (
                x       REAL,
                y       REAL,
                z       REAL,
                heading REAL
            );
        END if;
    END
$$;

ALTER TABLE users
    ADD COLUMN position character_position_type NULL; 