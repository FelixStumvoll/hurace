import React, { useState, useEffect } from 'react';
import { RaceRanking } from '../../../../interfaces/RaceRanking';
import { setStateAsync } from '../../../../common/stateSetter';
import { getRankingForRace } from '../../../../api';
import { RankingViewItem } from './RankingViewItem';
import styled from 'styled-components';
import { HeaderCard } from '../../../HeaderCard';

const RankingTable = styled.table`
    width: 100%;
`;

export const RankingView: React.FC<{ raceId: number }> = ({ raceId }) => {
    const [raceRanking, setRaceRanking] = useState<RaceRanking[] | undefined>(
        undefined
    );

    useEffect(() => {
        if (raceRanking !== undefined) return;
        setStateAsync(setRaceRanking, getRankingForRace(raceId));
    }, [raceRanking, raceId]);

    return (
        <HeaderCard headerText="Rangliste">
            <RankingTable>
                <thead>
                    <tr>
                        <th align="left" >Pos.</th>
                        <th align="left">Startnr.</th>
                        <th align="left">Land</th>
                        <th align="left">Name</th>
                        <th align="left">Zeit</th>
                        <th align="left">Rückstand</th>
                    </tr>
                </thead>
                <tbody>
                    {raceRanking?.map(rr => (
                        <RankingViewItem
                            key={rr.startList.startNumber}
                            raceRanking={rr}
                        />
                    ))}
                </tbody>
            </RankingTable>
        </HeaderCard>
    );
};
