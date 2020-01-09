import React from 'react';
import { HeaderCard } from '../../shared/HeaderCard';
import styled from 'styled-components';
import { getWinnersForRace } from '../../../common/api';
import { useStateAsync } from '../../../hooks/useStateAsync';
import { SkierListViewItem } from '../../skier-list-view/SkierListViewItem';
import { RaceRanking } from '../../../models/RaceRanking';

const WinnersFlex = styled.div`
    display: flex;
`;

const WinnerWrapper = styled.div`
    margin-left: ${props => props.theme.gap};

    :first-child {
        margin-left: 0;
    }
`;

export const RaceWinnersView: React.FC<{ winners: RaceRanking[] }> = ({
    winners
}) => {
    return (
        <HeaderCard headerText="Sieger">
            <WinnersFlex>
                {winners?.map(w => (
                    <WinnerWrapper key={w.position}>
                        <SkierListViewItem skier={w.startList.skier} />
                    </WinnerWrapper>
                ))}
            </WinnersFlex>
        </HeaderCard>
    );
};
