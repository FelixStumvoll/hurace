import React from 'react';
import { dummySeasons } from './dummySeasons';
import { SeasonViewItem } from './SeasonViewItem';
import styled from 'styled-components';

const SeasonItemPanel = styled.div`
    display: flex;
    flex-wrap: wrap;
`;

export const SeasonView: React.FC = () => {
    let seasons = dummySeasons;

    return (
        <SeasonItemPanel>
            {seasons.map(s => (
                <SeasonViewItem key={s.id} season={s} />
            ))}
        </SeasonItemPanel>
    );
};
