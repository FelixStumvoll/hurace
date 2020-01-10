import React from 'react';
import { RaceRanking } from '../../../../models/RaceRanking';
import { getTimeWithMS } from '../../../../common/timeConverter';
import {
    ListItem,
    VerticallyAlignedText,
    ColumnFlex,
    AlignRight
} from '../../../../theme/CustomComponents';
import styled from 'styled-components';

const RankingItem = styled(ListItem)`
    display: grid;
    grid-template-columns: 25px 0.5fr 1fr 1fr auto;
    gap: ${props => props.theme.gap};
`;

const Position = styled(VerticallyAlignedText)`
    font-weight: bold;
    width: fit-content;
`;

const SkierImage = styled.img`
    border-radius: 50%;
    height: 50px;
`;

const NameFlex = styled(ColumnFlex)`
    justify-content: center;
`;

const Country = styled.span`
    color: ${props => props.theme.gray};
    font-style: italic;
    font-size: 12px;
`;

export const RankingListViewItem: React.FC<{ raceRanking: RaceRanking }> = ({
    raceRanking
}) => {
    return (
        <RankingItem>
            <Position>{raceRanking.position}</Position>

            <AlignRight>
                <SkierImage src={raceRanking.startList.skier.imageUrl} />
            </AlignRight>
            <NameFlex>
                <span>
                    {raceRanking.startList.skier.firstName}{' '}
                    {raceRanking.startList.skier.lastName}
                </span>
                <Country>
                    {raceRanking.startList.skier.country?.countryCode}
                </Country>
            </NameFlex>

            <VerticallyAlignedText>
                {raceRanking.disqualified
                    ? '-'
                    : getTimeWithMS(raceRanking.time!)}
            </VerticallyAlignedText>
            <VerticallyAlignedText>
                {raceRanking.disqualified
                    ? '-'
                    : `+${getTimeWithMS(raceRanking.timeToLeader!)}`}
            </VerticallyAlignedText>
        </RankingItem>
    );
};
