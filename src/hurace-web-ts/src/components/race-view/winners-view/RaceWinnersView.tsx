import React from 'react';
import { HeaderCard } from '../../shared/HeaderCard';
import styled from 'styled-components';
import { getWinnersForRace } from '../../../common/api';
import { useStateAsync } from '../../../hooks/useStateAsync';
import { SkierListViewItem } from '../../skier-list-view/SkierViewItem';

const WinnersFlex = styled.div`
    display: flex;
`;

export const RaceWinnersView: React.FC<{ raceId: number }> = ({ raceId }) => {
    const [winners] = useStateAsync(getWinnersForRace, raceId);

    return (
        <HeaderCard headerText="Gewinner">
            <WinnersFlex>
                {winners?.map(w => (
                    <SkierListViewItem key={w.position} skier={w.startList.skier} />
                ))}
            </WinnersFlex>
        </HeaderCard>
    );
};
