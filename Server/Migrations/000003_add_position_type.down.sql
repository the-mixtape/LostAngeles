ALTER TABLE users
    DROP COLUMN IF EXISTS position;

DO
$$
    BEGIN
        IF EXISTS (SELECT 1 FROM pg_type WHERE typname = 'character_position_type') THEN
            DROP TYPE character_position_type;
        END IF;
    END
$$;