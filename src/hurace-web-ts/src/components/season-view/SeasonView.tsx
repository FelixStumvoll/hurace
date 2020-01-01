import React, { useState, useEffect } from 'react';
import { SeasonViewItem } from './SeasonViewItem';
import styled from 'styled-components';
import { Season } from '../../interfaces/Season';
import { setStateAsync } from '../../common/stateSetter';
import { getSeasons } from '../../api';

const SeasonItemPanel = styled.div`
    display: flex;
    flex-direction: column;
    flex-wrap: wrap;
`;

const SeasonLabel = styled.div`
    font-weight: bold;
    font-size: 20px;
    margin-bottom: 10px;
`;

export const SeasonView: React.FC = () => {
    const [seasons, setSeasons] = useState<Season[] | undefined>(undefined);

    useEffect(() => {
        if (seasons !== undefined) return;
        setStateAsync(setSeasons, getSeasons());
    }, [seasons]);

    return (
        <div>
            <SeasonLabel>Alle Saisonen:</SeasonLabel>
            <SeasonItemPanel>
                {seasons?.map(s => (
                    <SeasonViewItem key={s.id} season={s} />
                ))}
            </SeasonItemPanel>
        </div>
    );
};
